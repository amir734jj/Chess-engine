using System;
using System.ComponentModel;
using System.Net;
using ChessEngine.Models;
using ChessEngine.Models.Constants;
using ChessEngine.Models.Events;
using ChessEngine.Models.Pieces;
using Microsoft.VisualBasic.CompilerServices;

namespace ChessEngine.Logic
{
    /// <summary>
    /// A human player playing against an engine.
    /// </summary>
    abstract class HumanVsAI
    {
        /// <summary>
        /// The model.
        /// </summary>
        protected Game model;

        /// <summary>
        /// True if the game is in navigational mode (using next/prev/first/last), false otherwise.
        /// </summary>
        private bool navMode;

        /// <summary>
        /// The background worker.
        /// </summary>
        protected BackgroundWorker engineWorker;

        /// <summary>
        /// True to if the model loads moves, false otherwise.
        /// </summary>
        private bool isModelLoadingMoves;

        /// <summary>
        /// True if it's engine turn, false otherwise.
        /// </summary>
        internal bool AITurn =>
            model != null &&
            model.IsInitialized &&
            model.CurrentBoard.Status.WhiteTurn == OpponentsSettings.Default.AIOpponentPlaysWhite;

        public HumanVsAI()
        {
            // set the backgound worker
            engineWorker = new BackgroundWorker {WorkerSupportsCancellation = true};
            engineWorker.DoWork += engineWorker_DoWork;
            engineWorker.RunWorkerCompleted += engineWorker_RunWorkerCompleted;
        }

        /// <summary>
        /// Force engine to start thinking and move.
        /// </summary>
        internal void ForceAiMove()
        {
            if (model == null || !model.IsInitialized) { return; }

            // if the game has not ended and the engine has ended thinking
            if (!model.IsEnded && !engineWorker.IsBusy)
            {
                // exit navigational mode
                navMode = false;

                // request for AI turn
                RequestForAITurn();

                // start engine thinking
                var inputParams = GetEngineInputParams();
                engineWorker.RunWorkerAsync(inputParams);
            }
        }

        /// <summary>
        /// Aborts engine thinking.
        /// </summary>
        protected abstract void AbortAIMove();

        /// <summary>
        /// Starts the engine thinking.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void engineWorker_DoWork(object sender, DoWorkEventArgs e);

        /// <summary>
        /// Called when the engine ended thinking.
        /// Can throw an exception.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void engineWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                // try to make the move only if the move was not aborted
                if (!e.Cancelled)
                {
                    // if it was an error during the thinking, rethrow it
                    if (e.Error != null) { throw e.Error; }

                    // make the move
                    model.Make(e.Result as Move);
                }
            }
            catch (ArgumentException exc)
            {
                // the local engine or the web service made an illegal move
                MessageBox.Show(exc.Message, Resources.AIMovingTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (WebException)
            {
                // problems with the network
                MessageBox.Show(Resources.WSConnectionErrorMsg, Resources.AIMovingTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value></value>
        public Game Model
        {
            get => model;
            set
            {
                // the model can be set only once
                if (model == null && value != null)
                {
                    model = value;

                    // hook the events
                    model.Moved += model_Moved;
                    model.GoneForward += model_Navigate;
                    model.GoneBack += model_Navigate;
                    model.Modified += model_Modified;
                    model.BoardConfigurationLoaded += model_BoardConfigurationLoaded;
                    model.GameBoardConfigurationLoaded += model_GameBoardConfigurationLoaded;
                    model.GameMoveSectionLoaded += model_GameMoveSectionLoaded;

                    isModelLoadingMoves = false;
                }
            }
        }

        private void model_Moved(object sender, MoveEventArgs e)
        {
            if (isModelLoadingMoves) { return; }

            // if in navigational mode (possible only if the human player has moved)
            if (navMode)
            {
                // request for AI turn
                RequestForAITurn();

                // exit navigational mode
                navMode = false;
            }

            // if the game has not ended and if it is engine turn
            if (!model.IsEnded && AITurn)
            {
                // start engine thinking
                var inputParams = GetEngineInputParams();
                engineWorker.RunWorkerAsync(inputParams);
            }
        }

        private void model_Navigate(object sender, MoveEventArgs e)
        {
            // abort engine thinking
            AbortAIMove();

            // enter navigational mode
            navMode = true;
        }

        private void model_Modified(object sender, EventArgs e)
        {
            // abort engine thinking
            AbortAIMove();

            // enter navigational mode
            navMode = true;
        }

        private void model_BoardConfigurationLoaded(object sender, EventArgs e)
        {
            // abort engine thinking
            AbortAIMove();
        }

        private void model_GameBoardConfigurationLoaded(object sender, EventArgs e)
        {
            // stop receiving model move events
            isModelLoadingMoves = true;

            // abort engine thinking
            AbortAIMove();
        }

        private void model_GameMoveSectionLoaded(object sender, EventArgs e)
        {
            // start receiving model move events
            isModelLoadingMoves = false;
        }

        /// <summary>
        /// Gets the engine input parameters from the current game.
        /// </summary>
        /// <returns></returns>
        private EngineInputParameters GetEngineInputParams()
        {
            var inputParams = new EngineInputParameters {FEN = Utils.GetFEN(model.CurrentBoard)};

            // set the current board FEN

            // set the repetitive move candidate
            var repMove = model.RepetitiveMoveCandidate;
            inputParams.RepetitiveMoveCandidate = repMove != null ? Utils.GetCAN(repMove) : null;

            // set the depth level
            inputParams.DepthLevel = 10;

            return inputParams;
        }

        /// <summary>
        /// Dispose the engine resources.
        /// </summary>
        public virtual void Dispose()
        {
            // unhook the events
            if (model != null)
            {
                model.Moved -= model_Moved;
                model.GoneForward -= model_Navigate;
                model.GoneBack -= model_Navigate;
                model.Modified -= model_Modified;
                model.BoardConfigurationLoaded -= model_BoardConfigurationLoaded;
                model.GameBoardConfigurationLoaded -= model_GameBoardConfigurationLoaded;
                model.GameMoveSectionLoaded -= model_GameMoveSectionLoaded;
            }
        }
    }

    /// <summary>
    /// Helper class, stores the engine input parameters.
    /// </summary>
    public class EngineInputParameters
    {
        public string FEN { get; set; }

        public string RepetitiveMoveCandidate { get; set; }

        public int DepthLevel { get; set; }
    }
}
