// Matthew Kerr
// February 25, 2015


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameOfCraps
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {  
        private static int die1_value;
        private static int die2_value;
        private static int player_wins;
        private static int house_wins;
        private static int total;
        private static int bet;
        private static int point;
        private static int bank;

        private static bool gameElementsVisible;
        private static bool bankAmountElementsVisible;

        private static Rectangle[] dice_rectangles;
        private static Ellipse[] die1_ellipses;
        private static Ellipse[] die2_ellipses;
        private static Random rand;


        public MainWindow()
        {
            InitializeComponent();
            InitializeDice();
            die1_value = 0;
            die2_value = 0;
            player_wins = 0;
            house_wins = 0;
            total = 0;
            bet = 0;
            point = 0;
            bank = 0;
            rand = new Random();

            lbl_totalValue.Content = "";
            lbl_pointValue.Content = "";
            lbl_bankValue.Content = "";
            lbl_winner.Content = "";
            btn_rollDice.IsEnabled = false;
            btn_playAgain.IsEnabled = false;
            btn_resetSession.IsEnabled = false;
            mnu_rollDice.IsEnabled = false;
            mnu_playAgain.IsEnabled = false;
            mnu_resetSession.IsEnabled = false;

            BankAmountElementsVisible = true;
            GameElementsVisible = false;
        }

        private void InitializeDice()
        {
            // initialize rectangles
            dice_rectangles = new Rectangle[2];

            dice_rectangles[0] = rect_die1;
            dice_rectangles[1] = rect_die2;

            // initialize ellipses
            die1_ellipses = new Ellipse[7];
            die2_ellipses = new Ellipse[7];

            die1_ellipses[0] = ell_die1TopLeft;
            die1_ellipses[1] = ell_die1MidLeft;
            die1_ellipses[2] = ell_die1BotLeft;
            die1_ellipses[3] = ell_die1TopRight;
            die1_ellipses[4] = ell_die1MidRight;
            die1_ellipses[5] = ell_die1BotRight;
            die1_ellipses[6] = ell_die1Mid;

            die2_ellipses[0] = ell_die2TopLeft;
            die2_ellipses[1] = ell_die2MidLeft;
            die2_ellipses[2] = ell_die2BotLeft;
            die2_ellipses[3] = ell_die2TopRight;
            die2_ellipses[4] = ell_die2MidRight;
            die2_ellipses[5] = ell_die2BotRight;
            die2_ellipses[6] = ell_die2Mid;

            // hide ellipses
            for (int i = 0; i < 7; i++)
            {
                die1_ellipses[i].Visibility = Visibility.Hidden;
                die2_ellipses[i].Visibility = Visibility.Hidden;
            }
        }

        private void RollDice()
        {
            btn_resetSession.IsEnabled = false;
            textBx_betAmount.IsEnabled = false;
            btn_betAmountSubmit.IsEnabled = false;
            ResetDiceDots();

            die1_value = rand.Next(1, 7);
            die2_value = rand.Next(1, 7);
            total = die1_value + die2_value;

            DisplayDice(die1_ellipses, die1_value);
            DisplayDice(die2_ellipses, die2_value);

            UpdateTotal();

            UpdateGameState();
        }

        private void ResetRound()
        {
            point = 0;
            bet = 0;
            textBx_betAmount.Text = "0";
            ResetDiceDots();
            btn_rollDice.IsEnabled = true;
            mnu_rollDice.IsEnabled = true;
            btn_playAgain.IsEnabled = false;
            mnu_playAgain.IsEnabled = false;
            //btn_resetSession.IsEnabled = false;
            //mnu_resetSession.IsEnabled = false;
            btn_betAmountSubmit.IsEnabled = true;
            textBx_betAmount.IsEnabled = true;
            lbl_bankValue.Content = bank;
            lbl_totalValue.Content = "";
            lbl_pointValue.Content = "";
            lbl_winner.Content = "";
        }

        private void UpdateWins()
        {
            btn_rollDice.IsEnabled = false;
            mnu_rollDice.IsEnabled = false;
            btn_playAgain.IsEnabled = true;
            mnu_playAgain.IsEnabled = true;
            btn_resetSession.IsEnabled = true;
            mnu_resetSession.IsEnabled = true;

            lbl_bankValue.Content = bank;
            lbl_playerWinsValue.Content = player_wins;
            lbl_houseWinsValue.Content = house_wins;
            if (bank == 0)
            {
                GameOver();
            }
        }

        private void UpdateGameState()
        {
            if (point == 0)
            {
                // no point yet

                // player wins
                if (total == 7 || total == 11)
                {
                    player_wins++;
                    bank += (bet * 2);
                    lbl_winner.Content = "Winner!";
                    UpdateWins();
                }
                // house wins
                else if (total == 2 || total == 3 || total == 12)
                {
                    house_wins++;
                    lbl_winner.Content = "House Wins!";
                    UpdateWins();
                }
                // need point
                else
                {
                    point = total;
                    lbl_pointValue.Content = point;
                }
            }
            else
            {
                // have a point value
                if (total == 7)
                {
                    house_wins++;
                    lbl_winner.Content = "House Wins!";
                    UpdateWins();
                }
                else if (total == point)
                {
                    player_wins++;
                    bank += (bet * 2);
                    lbl_winner.Content = "Winner!";
                    UpdateWins();
                }
            }
        }

        private void GameOver()
        {
            if (bank == 0)
            {
                MessageBox.Show("Ran out of money!","Game Over");
            }
            else
            {
                string msg = "Went home with $" + bank + "!";
                MessageBox.Show(msg, "Game Over");
            }
            bank = 0;
            bet = 0;
            total = 0;
            point = 0;
            player_wins = 0;
            house_wins = 0;
            lbl_totalValue.Content = "";
            lbl_pointValue.Content = "";
            lbl_playerWinsValue.Content = player_wins;
            lbl_houseWinsValue.Content = house_wins;
            textBx_bankAmount.Text = "0";
            lbl_winner.Content = "";
            textBx_betAmount.IsEnabled = true;
            textBx_betAmount.Text = "0";
            btn_betAmountSubmit.IsEnabled = true;
            btn_rollDice.IsEnabled = false;
            btn_playAgain.IsEnabled = false;
            btn_resetSession.IsEnabled = false;
            mnu_rollDice.IsEnabled = false;
            mnu_playAgain.IsEnabled = false;
            mnu_resetSession.IsEnabled = false;

            ResetDiceDots();
            GameElementsVisible = false;
            BankAmountElementsVisible = true;
        }

        private void ResetDiceDots()
        {
            // hide ellipses
            for (int i = 0; i < 7; i++)
            {
                die1_ellipses[i].Visibility = Visibility.Hidden;
                die2_ellipses[i].Visibility = Visibility.Hidden;
            }
        }

        private void DisplayDice(Ellipse[] die_ellipses, int value)
        {
            if (value == 1)
            {
                die_ellipses[6].Visibility = Visibility.Visible;
            }
            else if (value == 2)
            {
                die_ellipses[0].Visibility = Visibility.Visible;
                die_ellipses[5].Visibility = Visibility.Visible;
            }
            else if (value == 3)
            {
                die_ellipses[2].Visibility = Visibility.Visible;
                die_ellipses[3].Visibility = Visibility.Visible;
                die_ellipses[6].Visibility = Visibility.Visible;
            }
            else if (value == 4)
            {
                die_ellipses[0].Visibility = Visibility.Visible;
                die_ellipses[2].Visibility = Visibility.Visible;
                die_ellipses[3].Visibility = Visibility.Visible;
                die_ellipses[5].Visibility = Visibility.Visible;
            }
            else if (value == 5)
            {
                die_ellipses[0].Visibility = Visibility.Visible;
                die_ellipses[2].Visibility = Visibility.Visible;
                die_ellipses[3].Visibility = Visibility.Visible;
                die_ellipses[5].Visibility = Visibility.Visible;
                die_ellipses[6].Visibility = Visibility.Visible;
            }
            else if (value == 6)
            {
                die_ellipses[0].Visibility = Visibility.Visible;
                die_ellipses[1].Visibility = Visibility.Visible;
                die_ellipses[2].Visibility = Visibility.Visible;
                die_ellipses[3].Visibility = Visibility.Visible;
                die_ellipses[4].Visibility = Visibility.Visible;
                die_ellipses[5].Visibility = Visibility.Visible;
            }
            else
            {
                // invalid number
                ResetDiceDots();
            }
        }

        private void StartGame()
        {
            BankAmountElementsVisible = false;
            GameElementsVisible = true;
            btn_rollDice.IsEnabled = true;
            mnu_rollDice.IsEnabled = true;
            btn_playAgain.IsEnabled = false;
            mnu_playAgain.IsEnabled = false;
            btn_resetSession.IsEnabled = true;
            mnu_resetSession.IsEnabled = true;
        }

        private void UpdateTotal()
        {
            lbl_totalValue.Content = total;
        }

        private bool IsNumeric(string text)
        {
            int n;
            bool isNumeric = int.TryParse(text, out n);
            if (n == 0)
            {
                return false;
            }
            return true;
        }

        public bool GameElementsVisible
        {
            get
            {
                return gameElementsVisible;
            }
            set
            {
                if (value == true)
                {
                    gameElementsVisible = true;

                    // dice rectangles
                    dice_rectangles[0].Visibility = Visibility.Visible;
                    dice_rectangles[1].Visibility = Visibility.Visible;               

                    // roll dice and play again buttons
                    btn_rollDice.Visibility = Visibility.Visible;
                    btn_playAgain.Visibility = Visibility.Visible;

                    // bet amounts
                    lbl_betAmount.Visibility = Visibility.Visible;
                    textBx_betAmount.Visibility = Visibility.Visible;
                    btn_betAmountSubmit.Visibility = Visibility.Visible;

                    // current roll group box
                    groupBx_currentRoll.Visibility = Visibility.Visible;

                    // bank labels
                    lbl_bank.Visibility = Visibility.Visible;
                    rect_bank.Visibility = Visibility.Visible;
                    lbl_bankValue.Visibility = Visibility.Visible;

                    // wins total group box
                    groupBx_winsTotal.Visibility = Visibility.Visible;

                    // reset session button
                    btn_resetSession.Visibility = Visibility.Visible;
                }
                else
                {
                    gameElementsVisible = false;

                    // dice rectangles
                    dice_rectangles[0].Visibility = Visibility.Hidden;
                    dice_rectangles[1].Visibility = Visibility.Hidden;

                    

                    // roll dice and play again buttons
                    btn_rollDice.Visibility = Visibility.Hidden;
                    btn_playAgain.Visibility = Visibility.Hidden;

                    // bet amounts
                    lbl_betAmount.Visibility = Visibility.Hidden;
                    textBx_betAmount.Visibility = Visibility.Hidden;
                    btn_betAmountSubmit.Visibility = Visibility.Hidden;

                    // current roll group box
                    groupBx_currentRoll.Visibility = Visibility.Hidden;

                    // bank labels
                    lbl_bank.Visibility = Visibility.Hidden;
                    rect_bank.Visibility = Visibility.Hidden;
                    lbl_bankValue.Visibility = Visibility.Hidden;

                    // wins total group box
                    groupBx_winsTotal.Visibility = Visibility.Hidden;

                    // reset session button
                    btn_resetSession.Visibility = Visibility.Hidden;
                }
            }
        }


        public bool BankAmountElementsVisible
        {
            get
            {
                return bankAmountElementsVisible;
            }
            set
            {
                if (value == true)
                {
                    bankAmountElementsVisible = true;
                    lbl_bankAmount.Visibility = Visibility.Visible;
                    textBx_bankAmount.Visibility = Visibility.Visible;
                    btn_bankAmountSubmit.Visibility = Visibility.Visible;
                }
                else
                {
                    bankAmountElementsVisible = false;
                    lbl_bankAmount.Visibility = Visibility.Hidden;
                    textBx_bankAmount.Visibility = Visibility.Hidden;
                    btn_bankAmountSubmit.Visibility = Visibility.Hidden;
                }
            }
        }

        private void CanRollDice(object sender, CanExecuteRoutedEventArgs e)
        {
            if (btn_rollDice.IsEnabled)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void RollDiceExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            RollDice();
        }

        private void CanPlayAgain(object sender, CanExecuteRoutedEventArgs e)
        {
            if (btn_playAgain.IsEnabled)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void PlayAgainExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ResetRound();
        }

        private void CanResetSession(object sender, CanExecuteRoutedEventArgs e)
        {
            if (btn_resetSession.IsEnabled)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void ResetSessionExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            GameOver();
        }

        private void CanQuit(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void QuitExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void CanHowToPlay(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void HowToPlayExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            string msg = "Rules of Craps:\n"
                       + "A player rolls two dice where each die has six faces in the usual way (values 1 through 6).\n"
                       + "After the dice have come to rest the sum of the two upward faces is calculated.\n\n"
                       + "The first roll:\n"
                       + "If the sum is 7 or 11 on the first throw the roller/player wins.\n"
                       + "If the sum is 2, 3 or 12 the roller/player loses, that is the house wins.\n"
                       + "If the sum is 4, 5, 6, 8, 9, or 10, that sum becomes the roller/player's \"point\".\n\n"
                       + "Continue given the player's point:\n"
                       + "Now the player must roll the \"point\" total before rolling a 7 in order to win.\n"
                       + "If they roll a 7 before rolling the point value they got on the first roll the roller/player loses (the 'house' wins).\n\n"
                       + "Betting:\n"
                       + "Enter a whole number ranging from 0 through the current bank amount\n"
                       + "and press 'Submit'.\n"
                       + "This will immediately take your bet amount out of the bank.\n"
                       + "If you win the round you double your bet.\n"
                       + "If you lose the round you lose your bet.\n"
                       + "You are permitted to bet 0 and play for fun.\n"
                       + "If your bank reaches 0 the game is over.";

            MessageBox.Show(msg, "How To Play");
        }

        private void CanAbout(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AboutExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            string msg = "Matthew Kerr\n"
                       + "Eastern Washington University\n"
                       + "Winter 2015\n\n"
                       + "Version 1.0.0.0\n"
                       + ".NET Framework v4.5\n"
                       + "Compiled for 32-bit systems";
            MessageBox.Show(msg, "About");
        }

        private void btn_rollDice_Click(object sender, RoutedEventArgs e)
        {
            RollDice();
        }

        private void btn_bankAmountSubmit_Click(object sender, RoutedEventArgs e)
        {
            int temp;
            bool parsed = int.TryParse(textBx_bankAmount.Text, out temp);
            if (parsed)
            {
                if (temp > 0)
                {
                    bank = temp;
                    lbl_bankValue.Content = bank;
                    StartGame();
                }
                else
                {
                    MessageBox.Show("Bank Amount must be a whole number greater than 0.", "Invalid Bank Amount");
                }


            }
            else
            {
                MessageBox.Show("Bank Amount must be a whole number greater than 0.", "Invalid Bank Amount");
            }
        }

        private void btn_playAgain_Click(object sender, RoutedEventArgs e)
        {
            ResetRound();
        }

        private void btn_resetSession_Click(object sender, RoutedEventArgs e)
        {
            GameOver();
        }

        private void btn_betAmountSubmit_Click(object sender, RoutedEventArgs e)
        {
            int temp;
            bool parsed = int.TryParse(textBx_betAmount.Text, out temp);
            if (parsed)
            {
                if (temp > bank)
                {
                    MessageBox.Show("Cannot bet more than you have in the bank.", "Invalid Bet");
                }
                else if (temp < 0)
                {
                    MessageBox.Show("Cannot bet a negative amount.", "Invalid Bet");
                }
                else if (temp == 0)
                {
                    // do nothing
                }
                else
                {
                    bet = temp;
                    bank -= bet;
                    lbl_bankValue.Content = bank;
                    btn_betAmountSubmit.IsEnabled = false;
                    textBx_betAmount.IsEnabled = false;
                    btn_resetSession.IsEnabled = false;
                    mnu_resetSession.IsEnabled = false;
                }
            }
            else
            {
                MessageBox.Show("Bet must be 0 or a whole number less than or equal to your bank amount.", "Invalid Bet");
            }
        }         
    }   
}
