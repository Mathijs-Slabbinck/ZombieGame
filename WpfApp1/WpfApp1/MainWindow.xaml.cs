using System.Diagnostics.Eventing.Reader;
using System.Printing;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary1;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region global variables
        string room = "entryway";
        string dayOfWeek = "";

        bool toiletLookedAround = false;
        bool toilet2LookedAround = false;
        bool toilet3LookedAround = false;
        bool hasFrontDoorKey = false;
        bool hasBackDoorKey = false;
        bool victory = false;

        int zombieLvl;
        int zombieHp = 100;
        int playerHP = 100;
        int XP = 0;
        int level = 1;
        int knifeLvl = 1;
        int gunLvl = 0;
        int tntAmount = 0;
        int bulletAmount = 0;
        int coinAmount = 0;
        int junkAmount = 0;
        int foodAmount = 0;
        // HOLDS IN ORDER: gunpowder, metal, wood, rope, lighter, duct tape, plastic
        int[] craftingMaterials = { 0, 0, 0, 0, 0, 0, 0 };

        Random rnd = new Random();
        #endregion



        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private int ReturnRandom(int min, int max)
        {

            int randomNumber = rnd.Next(min, max + 1);
            return randomNumber;
        }

        private void InitializeGame()
        {
            lblPlaceAt.Content = "ENTRYWAY";
            lstCraftingTableOrShop.Visibility = Visibility.Collapsed;
            btnStab.Visibility = Visibility.Collapsed;
            btnShoot.Visibility = Visibility.Collapsed;
            btnTnt.Visibility = Visibility.Collapsed;
            lblDisplayHp.Visibility = Visibility.Collapsed;
            lblDisplayLvl.Visibility = Visibility.Collapsed;
            lblEnemyLevel.Visibility = Visibility.Collapsed;
            lblEnemyHp.Visibility = Visibility.Collapsed;
            SetDayOfTheWeek();
            PrepNextRoom(true, false, true, false, true, false, true, false);
        }

        private void SetDayOfTheWeek()
        {
            DateTime Today = DateTime.Now;
            dayOfWeek = Today.DayOfWeek.ToString();
            lblWeekday.Content = dayOfWeek;
        }

        private void SetupCraftingTable()
        {
            lstCraftingTableOrShop.Items.Clear();
            lstCraftingTableOrShop.Items.Add("Upgrade Knife (" + Math.Floor(knifeLvl * 1.5) + " wood + " + Math.Floor(knifeLvl * 1.5) + " metal + " + knifeLvl + " duct tape)");
            if (gunLvl > 0)
            {
                lstCraftingTableOrShop.Items.Add("Upgrade Gun (" + Math.Floor(gunLvl * 1.5) + " metal + " + Math.Floor(gunLvl * 1.5) + " plastic + " + gunLvl + " duct tape)");
            }
            lstCraftingTableOrShop.Items.Add("Craft bullet ( 1 gunpowder + 1 metal)");
            lstCraftingTableOrShop.Items.Add("Craft TNT ( 1 gunpowder + 1 rope + 1 lighter + duct tape)");
            lstCraftingTableOrShop.Visibility = Visibility.Visible;
        }

        // FINISH THIS METHOD
        private void SetupShop()
        {
            lstCraftingTableOrShop.Items.Clear();
            if (gunLvl == 0)
            {
                lstCraftingTableOrShop.Items.Add("Buy gun (15 coins)");
            }
            lstCraftingTableOrShop.Items.Add("");
            lstCraftingTableOrShop.Items.Add("");
            lstCraftingTableOrShop.Items.Add("");
            lstCraftingTableOrShop.Items.Add("");
            lstCraftingTableOrShop.Visibility = Visibility.Visible;
        }

        private void LstCommands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCommands.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                string selectedCmd = lstCommands.SelectedItem.ToString();
                switch (room)
                {
                    case "entryway":
                        if (selectedCmd == "Go north!")
                        {
                            GoToHallWay1();
                        }
                        else if (selectedCmd == "Go east!")
                        {
                            GoToHallWay2();
                        }
                        else if (selectedCmd == "Go south!")
                        {
                            if (hasFrontDoorKey)
                            {
                                GoToDriveWay();
                            }
                            else
                            {
                                lblOutPut.Content = "You need the front door key to open this!";
                            }
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToToilet1();
                        }
                        else if (selectedCmd == "Look around!")
                        {
                            lblOutPut.Content = "Nothing special to see here!";
                        }
                        break;
                    case "toilet1":
                        if (selectedCmd == "Go east!")
                        {
                            GoToEntryWay();
                        }
                        else if (selectedCmd == "Look around!")
                        {
                            if (toiletLookedAround == false)
                            {
                                int randomCoinAmount = ReturnRandom(1, 3);
                                lblOutPut.Content = "You found " + randomCoinAmount + " coin(s) in the toilet!\nYou also found a bin!";
                                coinAmount += randomCoinAmount;
                                lblCoinsCount.Content = coinAmount;
                                toiletLookedAround = true;
                                OpenContainer("bin");
                            }
                            else
                            {
                                lblOutPut.Content = "You already found everything here!";
                            }
                        }
                        break;
                    case "toilet2":
                        if (selectedCmd == "Go east!")
                        {
                            GoToIndoorPool();
                        }
                        else if (selectedCmd == "Look around!")
                        {
                            if (toilet2LookedAround == false)
                            {
                                int randomCoinAmount = ReturnRandom(1, 3);
                                lblOutPut.Content = "You found " + randomCoinAmount + " coin(s) in the toilet!\nYou also found a bin!";
                                coinAmount += randomCoinAmount;
                                lblCoinsCount.Content = coinAmount;
                                toiletLookedAround = true;
                                OpenContainer("bin");
                            }
                            else
                            {
                                lblOutPut.Content = "You already found everything here!";
                            }
                        }
                        break;
                    case "toilet3":
                        if (selectedCmd == "Go south!")
                        {
                            GoToManCave();
                        }
                        else if (selectedCmd == "Look around!")
                        {
                            if (toilet3LookedAround == false)
                            {
                                int randomCoinAmount = ReturnRandom(1, 3);
                                lblOutPut.Content = "You found " + randomCoinAmount + " coin(s) in the toilet!\nYou also found a bin!";
                                coinAmount += randomCoinAmount;
                                lblCoinsCount.Content = coinAmount;
                                toiletLookedAround = true;
                                OpenContainer("bin");
                            }
                            else
                            {
                                lblOutPut.Content = "You already found everything here!";
                            }

                        }
                        break;
                    case "hallway1":
                        if (selectedCmd == "Go north-west!")
                        {
                            GoToBedRoom1();
                        }
                        else if (selectedCmd == "Go east!")
                        {
                            GoToHallWayJunction();
                        }
                        else if (selectedCmd == "Go south!")
                        {
                            GoToEntryWay();
                        }
                        else if (selectedCmd == "Go South-east!")
                        {
                            GoToBedRoom2();
                        }
                        else if (selectedCmd == "Go West!")
                        {
                            GoToHomeOffice();
                        }

                        break;
                    case "hallway2":
                        if (selectedCmd == "Go north!")
                        {
                            GoToHallWayJunction();
                        }
                        else if (selectedCmd == "Go east!")
                        {
                            GoToDiningRoom();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToEntryWay();
                        }
                        break;
                    case "hallway3":
                        if (selectedCmd == "Go north!")
                        {
                            GoToHallWay4();
                        }
                        else if (selectedCmd == "Go north-east!")
                        {
                            GoToWineCellar();
                        }
                        else if (selectedCmd == "Go south-east!")
                        {
                            GoToManCave();
                        }
                        else if (selectedCmd == "Go south!")
                        {
                            GoToDiningRoom();
                        }
                        else if (selectedCmd == "Go south-west!")
                        {
                            GoToHallWayJunction();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToIndoorPool();
                        }
                        break;
                    case "hallway4":
                        if (selectedCmd == "Go north!")
                        {
                            GoToGym();
                        }
                        else if (selectedCmd == "Go east!")
                        {
                            GoToLivingRoom();
                        }
                        else if (selectedCmd == "Go south!")
                        {
                            GoToHallWay3();
                        }
                        else if (selectedCmd == "Go south-west!")
                        {
                            GoToIndoorPool();
                        }
                        else if (selectedCmd == "Go north-west!")
                        {
                            GoToHallWay5();
                        }
                        break;
                    case "hallway5":
                        if (selectedCmd == "Go east!")
                        {
                            GoToHallWay4();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToLibrary();
                        }
                        break;
                    case "hallway6":
                        if (selectedCmd == "Go east!")
                        {
                            GoToLibrary();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToWorkPlace();
                        }
                        break;
                    case "hallwayJunction":
                        if (selectedCmd == "Go north!")
                        {
                            GoToIndoorPool();
                        }
                        else if (selectedCmd == "Go east!")
                        {
                            GoToHallWay3();
                        }
                        else if (selectedCmd == "Go South!")
                        {
                            GoToHallWay2();
                        }
                        else if (selectedCmd == "Go east!")
                        {
                            GoToHallWay1();
                        }
                        break;
                    case "diningRoom":
                        if (selectedCmd == "Go north!")
                        {
                            GoToHallWay3();
                        }
                        else if (selectedCmd == "Go east!")
                        {
                            GoToKitchen();
                        }
                        else if (selectedCmd == "Go south!")
                        {
                            GoToPantry();
                        }
                        else if (selectedCmd == "Go east!")
                        {
                            GoToHallWay2();
                        }
                        break;
                    case "pantry":
                        if (selectedCmd == "Go north-east!")
                        {
                            GoToKitchen();
                        }
                        else if (selectedCmd == "Go north-west!")
                        {
                            GoToDiningRoom();
                        }
                        break;
                    case "kitchen":
                        if (selectedCmd == "Go north!")
                        {
                            GoToManCave();
                        }
                        else if (selectedCmd == "Go south!")
                        {
                            GoToPantry();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToDiningRoom();
                        }
                        break;
                    case "manCave":
                        if (selectedCmd == "Go north!")
                        {
                            GoToToilet3();
                        }
                        else if (selectedCmd == "Go south!")
                        {
                            GoToKitchen();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToHallWay3();
                        }
                        break;
                    case "indoorPool":
                        if (selectedCmd == "Go north-east!")
                        {
                            GoToHallWay4();
                        }
                        else if (selectedCmd == "Go south-east!")
                        {
                            GoToHallWay3();
                        }
                        else if (selectedCmd == "Go south!")
                        {
                            GoToHallWayJunction();
                        }
                        else if (selectedCmd == "Go south-west!")
                        {
                            GoToToilet2();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToBathRoom2();
                        }
                        else if (selectedCmd == "Go north-west!")
                        {
                            GoToCleaningRoom();
                        }
                        break;
                    case "bedroom1":
                        if (selectedCmd == "Go south!")
                        {
                            GoToHallWay1();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToHomeOffice();
                        }
                        break;
                    case "bedroom2":
                        if (selectedCmd == "Go north!")
                        {
                            GoToHallWay1();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToHomeOffice();
                        }
                        break;
                    case "guestBedroom":
                        if (selectedCmd == "Go north!")
                        {
                            GoToHomeOffice();
                        }
                        else if (selectedCmd == "Go east!")
                        {
                            GoToBathRoom3();
                        }
                        break;
                    case "homeOffice":
                        if (selectedCmd == "Go north!")
                        {
                            GoToSpa();
                        }
                        else if (selectedCmd == "Go north-east!")
                        {
                            GoToBedRoom1();
                        }
                        else if (selectedCmd == "Go east!")
                        {
                            GoToHallWay1();
                        }
                        else if (selectedCmd == "Go south-east!")
                        {
                            GoToBedRoom2();
                        }
                        else if (selectedCmd == "Go south!")
                        {
                            GoToGuestBedroom();
                        }
                        break;
                    case "bathRoom1":
                        if (selectedCmd == "Go east!")
                        {
                            GoToGym();
                        }
                        break;
                    case "bathRoom2":
                        if (selectedCmd == "Go east!")
                        {
                            GoToIndoorPool();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToSpa();
                        }
                        break;
                    case "bathRoom3":
                        if (selectedCmd == "Go west!")
                        {
                            GoToGuestBedroom();
                        }
                        break;
                    case "cleaningRoom":
                        if (selectedCmd == "Go east!")
                        {
                            GoToIndoorPool();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToSpa();
                        }
                        break;
                    case "basement":
                        if (selectedCmd == "Go east!")
                        {
                            GoToLibrary();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToWorkPlace();
                        }
                        break;
                    case "gym":
                        if (selectedCmd == "Go south!")
                        {
                            GoToHallWay4();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToBathRoom1();
                        }
                        break;
                    case "library":
                        if (selectedCmd == "Go south-east!")
                        {
                            GoToHallWay5();
                        }
                        else if (selectedCmd == "Go south-west!")
                        {
                            GoToHallWay6();
                        }
                        else if (selectedCmd == "Go north-west!")
                        {
                            GoToBasement();
                        }
                        break;
                    case "spa":
                        if (selectedCmd == "Go north!")
                        {
                            GoToWorkPlace();
                        }
                        else if (selectedCmd == "Go north-east!")
                        {
                            GoToCleaningRoom();
                        }
                        else if (selectedCmd == "Go east!")
                        {
                            GoToBathRoom2();
                        }
                        else if (selectedCmd == "Go south!")
                        {
                            GoToHomeOffice();
                        }
                        break;
                    case "workPlace":
                        if (selectedCmd == "Go north!")
                        {
                            if (hasBackDoorKey)
                            {
                                GoToDriveWay();
                            }
                            else
                            {
                                lblOutPut.Content = "You need the backdoor key to open this!";
                            }
                        }
                        else if (selectedCmd == "Go north-east!")
                        {
                            GoToBasement();
                        }
                        else if (selectedCmd == "Go south-east!")
                        {
                            GoToHallWay6();
                        }
                        else if (selectedCmd == "Go south!")
                        {
                            GoToSpa();
                        }
                        break;
                    case "wineCellar":
                        if (selectedCmd == "Go north!")
                        {
                            GoToLivingRoom();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToHallWay3();
                        }
                        break;
                    case "livingRoom":
                        if (selectedCmd == "Go north!")
                        {
                            GoToAttic();
                        }
                        else if (selectedCmd == "Go south!")
                        {
                            GoToWineCellar();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToHallWay4();
                        }
                        break;
                    case "attic":
                        if (selectedCmd == "Go south!")
                        {
                            GoToLivingRoom();
                        }
                        break;
                    case "garden":
                        if (selectedCmd == "Go south!")
                        {
                            GoToWorkPlace();
                        }
                        break;
                    case "driveway":
                        if (selectedCmd == "Go north!")
                        {
                            GoToEntryWay();
                        }
                        else if (selectedCmd == "Go east!")
                        {
                            GoToGarage2();
                        }
                        else if (selectedCmd == "Go south!")
                        {
                            GoToRoad();
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToGarage1();
                        }
                        break;
                    case "garage1":
                        if (selectedCmd == "Go east!")
                        {
                            GoToDriveWay();
                        }
                        break;
                    case "garage2":
                        if (selectedCmd == "Go west!")
                        {
                            GoToDriveWay();
                        }
                        break;
                    case "road":
                        if (selectedCmd == "Go north!")
                        {
                            GoToDriveWay();
                        }
                        else if (selectedCmd == "Go east!")
                        {
                            GoToSouthEastForest();
                        }
                        else if (selectedCmd == "Go south!")
                        {
                            if (victory)
                            {
                                MessageBox.Show("Congrats you won THE GAME!");
                            }
                            else
                            {
                                lblOutPut.Content = "You need to defeat the boss in order to escape!";
                            }
                        }
                        else if (selectedCmd == "Go west!")
                        {
                            GoToSouthWestForest();
                        }
                        break;
                    case "westForest":
                        if (selectedCmd == "Go east!")
                        {
                            GoToSouthWestForest();
                        }
                        break;
                    case "eastForest":
                        if (selectedCmd == "Go west!")
                        {
                            GoToSouthEastForest();
                        }
                        break;

                        /*default:
                            MessageBox.Show("Something went wrong!");
                            break;*/
                }
            }
        }

        private void PrepNextRoom(bool north, bool northEast, bool east, bool southEast, bool south, bool southWest, bool west, bool northWest)
        {
            lstCommands.Items.Clear();
            if (north)
            {
                lstCommands.Items.Add("Go north!");
            }
            if (northEast)
            {
                lstCommands.Items.Add("Go north-east!");
            }
            if (east)
            {
                lstCommands.Items.Add("Go east!");
            }
            if (southEast)
            {
                lstCommands.Items.Add("Go north-east!");
            }
            if (south)
            {
                lstCommands.Items.Add("Go south!");
            }
            if (southWest)
            {
                lstCommands.Items.Add("Go south-west!");
            }
            if (west)
            {
                lstCommands.Items.Add("Go west!");
            }
            if (northWest)
            {
                lstCommands.Items.Add("Go north-west!");
            }
            lstCommands.Items.Add("Look around!");
            lblContainerContentText.Visibility = Visibility.Hidden;
            lstContainer.Visibility = Visibility.Hidden;
            btnCloseContainer.Visibility = Visibility.Hidden;
            lblOutPut.Content = "";
        }

        private void FillCmdsBack(string room)
        {

            switch (room)
            {
                case "hallway1":
                    PrepNextRoom(false, false, true, false, true, true, true, true);
                    break;
                case "hallway2":
                    PrepNextRoom(true, false, true, false, false, false, true, false);
                    break;
                case "hallway3":
                    PrepNextRoom(true, true, false, true, true, false, true, true);
                    break;
                case "hallway4":
                    PrepNextRoom(true, false, true, false, true, true, false, true);
                    break;
                case "hallway5":
                    PrepNextRoom(false, false, true, false, false, false, true, false);
                    break;
                case "hallway6":
                    PrepNextRoom(false, false, true, false, false, false, true, false);
                    break;
                case "hallwayJunction":
                    PrepNextRoom(true, false, true, false, true, false, true, false);
                    break;
                case "gym":
                    PrepNextRoom(false, false, false, false, true, false, true, false);
                    break;
                case "workPlace":
                    PrepNextRoom(true, true, false, true, true, false, false, false);
                    break;
                case "bathRoom1":
                    PrepNextRoom(false, false, true, false, false, false, false, false);
                    break;
                case "bathRoom2":
                    PrepNextRoom(false, false, true, false, false, false, true, false);
                    break;
                case "bathRoom3":
                    PrepNextRoom(false, false, false, false, false, false, true, false);
                    break;
                case "bedroom1":
                    PrepNextRoom(false, false, false, false, true, false, true, false);
                    break;
                case "bedroom2":
                    PrepNextRoom(true, false, false, false, false, false, true, false);
                    break;
                case "guestBedroom":
                    PrepNextRoom(true, false, true, false, false, false, false, false);
                    break;
                case "homeOffice":
                    PrepNextRoom(true, true, true, true, true, false, false, false);
                    break;
                case "spa":
                    PrepNextRoom(true, true, true, false, true, false, false, false);
                    break;
                case "indoorPool":
                    PrepNextRoom(false, true, false, true, true, true, true, true);
                    break;
                case "driveWay":
                    PrepNextRoom(true, false, true, false, true, false, true, false);
                    break;
                case "toilet1":
                    PrepNextRoom(false, false, true, false, false, false, false, false);
                    break;
                case "toilet2":
                    PrepNextRoom(false, false, true, false, false, false, false, false);
                    break;
                case "toilet3":
                    PrepNextRoom(false, false, false, false, true, false, false, false);
                    break;
                case "entryway":
                    PrepNextRoom(true, false, true, false, true, false, true, false);
                    break;
                case "diningRoom":
                    PrepNextRoom(true, false, true, false, true, false, true, false);
                    break;
                case "kitchen":
                    PrepNextRoom(true, false, false, false, true, false, true, false);
                    break;
                case "pantry":
                    PrepNextRoom(false, true, false, false, false, false, false, true);
                    break;
                case "manCave":
                    PrepNextRoom(true, false, false, false, true, false, true, false);
                    break;
                case "wineCellar":
                    PrepNextRoom(true, false, false, false, false, false, true, false);
                    break;
                case "livingRoom":
                    PrepNextRoom(true, false, false, false, true, false, true, false);
                    break;
                case "attic":
                    PrepNextRoom(false, false, false, false, true, false, false, false);
                    break;
                case "cleaningRoom":
                    PrepNextRoom(false, false, true, false, false, false, true, false);
                    break;
                case "garage1":
                    PrepNextRoom(false, false, true, false, false, false, false, false);
                    break;
                case "garage2":
                    PrepNextRoom(false, false, false, false, false, false, true, false);
                    break;
                case "library":
                    PrepNextRoom(false, false, false, true, false, true, false, true);
                    break;
                case "basement":
                    PrepNextRoom(false, false, true, false, false, false, true, false);
                    break;
                case "garden":
                    PrepNextRoom(false, false, false, false, false, true, false, false);
                    break;
                case "road":
                    PrepNextRoom(true, false, true, false, true, false, true, false);
                    break;
                case "southWestForest":
                    PrepNextRoom(false, false, true, false, false, false, true, false);
                    break;
                case "southEastForest":
                    PrepNextRoom(false, false, true, false, false, false, true, false);
                    break;
                case "eastForest":
                    PrepNextRoom(false, false, false, false, false, false, true, false);
                    break;
                case "westForest":
                    PrepNextRoom(false, false, true, false, false, false, false, false);
                    break;
            }
        }

        private void SpawnZombie()
        {
            if (ReturnRandom(1, 5) == 1)
            {
                Zombie zombie = new Zombie(level);

                lblOutPut.Content = "A zombie appeared!";
                lstCommands.Items.Clear();
                btnStab.Visibility = Visibility.Visible;
                btnShoot.Visibility = Visibility.Visible;
                btnTnt.Visibility = Visibility.Visible;
                lblDisplayHp.Visibility = Visibility.Visible;
                lblDisplayLvl.Visibility = Visibility.Visible;
                lblEnemyLevel.Visibility = Visibility.Visible;
                lblEnemyHp.Visibility = Visibility.Visible;
                lblEnemyHp.Content = zombie.Hp;
                lblEnemyLevel.Content = zombie.Lvl;
            }
        }

        #region prepare the rooms
        private void GoToHallWay1()
        {
            room = "hallway1";
            PrepNextRoom(false, false, true, false, true, true, true, true);
            lblPlaceAt.Content = "HALLWAY 1";
            SpawnZombie();
        }

        private void GoToHallWay2()
        {
            room = "hallway2";
            PrepNextRoom(true, false, true, false, false, false, true, false);
            lblPlaceAt.Content = "HALLWAY 2";
            SpawnZombie();
        }

        private void GoToHallWay3()
        {
            room = "hallway3";
            PrepNextRoom(true, true, false, true, true, false, true, true);
            lblPlaceAt.Content = "HALLWAY 3";
            SpawnZombie();
        }

        private void GoToHallWay4()
        {
            room = "hallway4";
            PrepNextRoom(true, false, true, false, true, true, false, true);
            lblPlaceAt.Content = "HALLWAY 4";
            SpawnZombie();
        }

        private void GoToHallWay5()
        {
            room = "hallway5";
            PrepNextRoom(false, false, true, false, false, false, true, false);
            lblPlaceAt.Content = "HALLWAY 5";
            SpawnZombie();
        }

        private void GoToHallWay6()
        {
            room = "hallway6";
            PrepNextRoom(false, false, true, false, false, false, true, false);
            lblPlaceAt.Content = "HALLWAY 6";
            SpawnZombie();
        }

        private void GoToHallWayJunction()
        {
            room = "hallwayJunction";
            PrepNextRoom(true, false, true, false, true, false, true, false);
            lblPlaceAt.Content = "HALLWAY JUNCTION";
            SpawnZombie();
        }

        private void GoToGym()
        {
            room = "gym";
            PrepNextRoom(false, false, false, false, true, false, true, false);
            lblPlaceAt.Content = "GYM";
            SpawnZombie();
        }

        private void GoToWorkPlace()
        {
            room = "workPlace";
            PrepNextRoom(true, true, false, true, true, false, false, false);
            lblPlaceAt.Content = "WORKPLACE";
            SpawnZombie();
        }
        private void GoToBathRoom1()
        {
            room = "bathRoom1";
            PrepNextRoom(false, false, true, false, false, false, false, false);
            lblPlaceAt.Content = "BATHROOM 1";
            SpawnZombie();
        }

        private void GoToBathRoom2()
        {
            room = "bathRoom2";
            PrepNextRoom(false, false, true, false, false, false, true, false);
            lblPlaceAt.Content = "BATHROOM 2";
            SpawnZombie();
        }

        private void GoToBathRoom3()
        {
            room = "bathRoom3";
            PrepNextRoom(false, false, false, false, false, false, true, false);
            lblPlaceAt.Content = "BATHROOM 3";
            SpawnZombie();
        }

        private void GoToBedRoom1()
        {
            room = "bedroom1";
            PrepNextRoom(false, false, false, false, true, false, true, false);
            lblPlaceAt.Content = "BEDROOM 1";
            SpawnZombie();
        }
        private void GoToBedRoom2()
        {
            room = "bedroom2";
            PrepNextRoom(true, false, false, false, false, false, true, false);
            lblPlaceAt.Content = "BEDROOM 2";
            SpawnZombie();
        }

        private void GoToGuestBedroom()
        {
            room = "guestBedroom";
            PrepNextRoom(true, false, true, false, false, false, false, false);
            lblPlaceAt.Content = "GUEST BEDROOM";
            SpawnZombie();
        }

        private void GoToHomeOffice()
        {
            room = "homeOffice";
            PrepNextRoom(true, true, true, true, true, false, false, false);
            lblPlaceAt.Content = "HOME OFFICE";
            SpawnZombie();
        }
        private void GoToSpa()
        {
            room = "spa";
            PrepNextRoom(true, true, true, false, true, false, false, false);
            lblPlaceAt.Content = "SPA\n(WELLNESS)";
            SpawnZombie();
        }

        private void GoToIndoorPool()
        {
            room = "indoorPool";
            PrepNextRoom(false, true, false, true, true, true, true, true);
            lblPlaceAt.Content = "INDOOR POOL";
            SpawnZombie();
        }

        private void GoToDriveWay()
        {
            room = "driveway";
            PrepNextRoom(true, false, true, false, true, false, true, false);
            lblPlaceAt.Content = "DRIVEWAY";
            SpawnZombie();
        }


        private void GoToToilet1()
        {
            room = "toilet1";
            PrepNextRoom(false, false, true, false, false, false, false, false);
            lblPlaceAt.Content = "TOILET 1";
            SpawnZombie();
        }

        private void GoToToilet2()
        {
            room = "toilet2";
            PrepNextRoom(false, false, true, false, false, false, false, false);
            lblPlaceAt.Content = "TOILET 2";
            SpawnZombie();
        }

        private void GoToToilet3()
        {
            room = "toilet3";
            PrepNextRoom(false, false, false, false, true, false, false, false);
            lblPlaceAt.Content = "TOILET 1";
            SpawnZombie();
        }

        private void GoToEntryWay()
        {
            room = "entryway";
            PrepNextRoom(true, false, true, false, true, false, true, false);
            lblPlaceAt.Content = "ENTERYWAY";
            SpawnZombie();
        }

        private void GoToDiningRoom()
        {
            room = "diningRoom";
            PrepNextRoom(true, false, true, false, true, false, true, false);
            lblPlaceAt.Content = "DINING ROOM";
            SpawnZombie();
        }

        private void GoToKitchen()
        {
            room = "kitchen";
            PrepNextRoom(true, false, false, false, true, false, true, false);
            lblPlaceAt.Content = "KITCHEN";
            SpawnZombie();
        }

        private void GoToPantry()
        {
            room = "pantry";
            PrepNextRoom(false, true, false, false, false, false, false, true);
            lblPlaceAt.Content = "PANTRY";
            SpawnZombie();
        }

        private void GoToManCave()
        {
            room = "manCave";
            PrepNextRoom(true, false, false, false, true, false, true, false);
            lblPlaceAt.Content = "MAN CAVE";
            SpawnZombie();
        }

        private void GoToWineCellar()
        {
            room = "wineCellar";
            PrepNextRoom(true, false, false, false, false, false, true, false);
            lblPlaceAt.Content = "WINE CELLAR";
            SpawnZombie();
        }

        private void GoToLivingRoom()
        {
            room = "livingRoom";
            PrepNextRoom(true, false, false, false, true, false, true, false);
            lblPlaceAt.Content = "LIVING ROOM";
            SpawnZombie();
        }

        private void GoToAttic()
        {
            room = "attic";
            PrepNextRoom(false, false, false, false, true, false, false, false);
            lblPlaceAt.Content = "UPSTAIRS\n(ATTIC)";
            SpawnZombie();
        }
        private void GoToCleaningRoom()
        {
            room = "cleaningRoom";
            PrepNextRoom(false, false, true, false, false, false, true, false);
            lblPlaceAt.Content = "CLEANING ROOM";
            SpawnZombie();
        }
        private void GoToGarage1()
        {
            room = "garage1";
            PrepNextRoom(false, false, true, false, false, false, false, false);
            lblPlaceAt.Content = "GARAGE 1";
            SpawnZombie();
        }

        private void GoToGarage2()
        {
            room = "garage2";
            PrepNextRoom(false, false, false, false, false, false, true, false);
            lblPlaceAt.Content = "GARAGE 2";
            SpawnZombie();
        }

        private void GoToLibrary()
        {
            room = "library";
            PrepNextRoom(false, false, false, true, false, true, false, true);
            lblPlaceAt.Content = "LIBRARY";
            SpawnZombie();
        }

        private void GoToBasement()
        {
            room = "basement";
            PrepNextRoom(false, false, true, false, false, false, true, false);
            lblPlaceAt.Content = "DOWNSTAIRS\n(BASEMENT)";
            SpawnZombie();
        }
        private void GoToGarden()
        {
            room = "garden";
            PrepNextRoom(false, false, false, false, false, true, false, false);
            lblPlaceAt.Content = "GARDEN";
            SpawnZombie();
        }

        private void GoToRoad()
        {
            room = "garden";
            PrepNextRoom(true, false, true, false, true, false, true, false);
            lblPlaceAt.Content = "GARDEN";
            SpawnZombie();
        }

        private void GoToSouthWestForest()
        {
            room = "southWestForest";
            PrepNextRoom(false, false, true, false, false, false, true, false);
            lblPlaceAt.Content = "SOUTH WEST FOREST";
            SpawnZombie();
        }
        private void GoToSouthEastForest()
        {
            room = "southEastForest";
            PrepNextRoom(false, false, true, false, false, false, true, false);
            lblPlaceAt.Content = "SOUTH EAST FOREST";
            SpawnZombie();
        }
        private void GoToEastForest()
        {
            room = "eastForest";
            PrepNextRoom(false, false, false, false, false, false, true, false);
            lblPlaceAt.Content = "EAST FOREST";
            SpawnZombie();
        }

        private void GoToWestForest()
        {
            room = "westForest";
            PrepNextRoom(false, false, true, false, false, false, false, false);
            lblPlaceAt.Content = "WEST FOREST";
            SpawnZombie();
        }

        #endregion

        #region handle containers

        private void BtnCloseContainer_Click(object sender, RoutedEventArgs e)
        {
            lblContainerContentText.Visibility = Visibility.Hidden;
            lstContainer.Visibility = Visibility.Hidden;
            btnCloseContainer.Visibility = Visibility.Hidden;
            for (int i = 0; i < lstCommands.Items.Count; i++)
            {
                if (lstCommands.Items[i].ToString() == "Look around!")
                {
                    lstCommands.Items.RemoveAt(i);
                    break; // Exit loop after removing the first match
                }
            }
        }

        private void OpenContainer(string containerType)
        {
            /* vv make content of chest visible vv */

            lblContainerContentText.Visibility = Visibility.Visible;
            lstContainer.Visibility = Visibility.Visible;
            btnCloseContainer.Visibility = Visibility.Visible;

            int[] RandomNumbers = new int[6]; // Initialize the array with a size of 6

            for (int i = 0; i < 5; i++)
            {
                RandomNumbers[i] = ReturnRandom(1, 10); // Assign a random integer to each element in the array
                switch (containerType)
                {
                    case "chest":
                        lstContainer.Items.Add(ReturnWhichItemToPutInChest(RandomNumbers[i]));
                        break;
                    case "bin":
                        lstContainer.Items.Add(ReturnWhichItemToPutInBin(RandomNumbers[i]));
                        break;
                    case "fridge":
                        lstContainer.Items.Add(ReturnWhichItemToPutInFridge(RandomNumbers[i]));
                        break;
                    case "toolbox":
                        lstContainer.Items.Add(ReturnWhichItemToPutInToolbox(RandomNumbers[i]));
                        break;
                    case "zombie":
                        lstContainer.Items.Add(ReturnWhichItemToPutInZombie(RandomNumbers[i]));
                        break;
                }
            }
        }

        #region return items to put in containers
        private string ReturnWhichItemToPutInChest(int randomNumber)
        {
            switch (randomNumber)
            {
                case 1:
                    int coinAmount = ReturnRandom(1, 4);
                    return coinAmount.ToString() + " coin(s)";
                case 2:
                    return "junk";
                case 3:
                    return "food";
                case 4:
                    return "gunpowder";
                case 5:
                    return "metal";
                case 6:
                    return "wood";
                case 7:
                    return "rope";
                case 8:
                    return "lighter";
                case 9:
                    return "duct tape";
                case 10:
                    return "plastic";
                default:
                    return "NULL";
            }
        }

        private string ReturnWhichItemToPutInZombie(int randomNumber)
        {
            switch (randomNumber)
            {
                case 1:
                    int coinAmount = ReturnRandom(2, 5);
                    return coinAmount.ToString() + " coin(s)";
                case 2:
                    return "junk";
                case 3:
                    return "junk";
                case 4:
                    return "junk";
                case 5:
                    return "gunpowder";
                case 6:
                    return "gunpowder";
                case 7:
                    return "junk";
                case 8:
                    return "metal";
                case 9:
                    return "plastic";
                case 10:
                    return "junk";
                default:
                    return "Null";
            }


        }

        private string ReturnWhichItemToPutInBin(int randomNumber)
        {
            switch (randomNumber)
            {
                case 1:
                    int coinAmount = ReturnRandom(1, 2);
                    return coinAmount.ToString() + " coin(s)";
                case 2:
                    return "junk";
                case 3:
                    return "junk";
                case 4:
                    return "gunpowder";
                case 5:
                    return "junk";
                case 6:
                    return "junk";
                case 7:
                    return "rope";
                case 8:
                    return "lighter";
                case 9:
                    return "duct tape";
                case 10:
                    return "metal";
                default:
                    return "NULL";
            }
        }

        private string ReturnWhichItemToPutInFridge(int randomNumber)
        {
            switch (randomNumber)
            {
                case 1:
                    int coinAmount = ReturnRandom(1, 2);
                    return coinAmount.ToString() + " coin(s)";
                    break;
                case 2:
                    return "food";
                    break;
                case 3:
                    return "food";
                    break;
                case 4:
                    return "gunpowder";
                    break;
                case 5:
                    return "food";
                    break;
                case 6:
                    return "food";
                    break;
                case 7:
                    return "junk";
                    break;
                case 8:
                    return "plastic";
                    break;
                case 9:
                    return "duct tape";
                    break;
                case 10:
                    return "food";
                    break;
                default:
                    return "NULL";
            }
        }

        private string ReturnWhichItemToPutInToolbox(int randomNumber)
        {
            switch (randomNumber)
            {
                case 1:
                    int coinAmount = ReturnRandom(1, 3);
                    return coinAmount.ToString() + " coin(s)";
                case 2:
                    return "metal";
                case 3:
                    return "junk";
                case 4:
                    return "wood";
                case 5:
                    return "lighter";
                case 6:
                    return "plastic";
                case 7:
                    return "metal";
                case 8:
                    return "rope";
                case 9:
                    return "duct tape";
                case 10:
                    return "metal";
                default:
                    return "NULL";
            }
        }
        #endregion

        private void LstContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstContainer.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                string whichItem = lstContainer.SelectedItem.ToString();

                /* vv CODE FOR COINS vv */
                Match match = Regex.Match(whichItem, @"(\d+) coin\(s\)");
                if (match.Success)
                {
                    int numberOfCoins = int.Parse(match.Groups[1].Value);
                    coinAmount += numberOfCoins;
                    lblCoinsCount.Content = coinAmount;
                    lstContainer.Items.RemoveAt(lstContainer.SelectedIndex);
                }
                else
                {
                    switch (whichItem)
                    {
                        case "junk":
                            junkAmount += 1;
                            lblJunkCount.Content = junkAmount;
                            lstContainer.Items.RemoveAt(lstContainer.SelectedIndex);
                            break;
                        case "food":
                            foodAmount += 1;
                            lblFoodCount.Content = foodAmount;
                            lstContainer.Items.RemoveAt(lstContainer.SelectedIndex);
                            break;
                        case "gunpowder":
                            craftingMaterials[0] += 1;
                            lblGunPowderCount.Content = craftingMaterials[0];
                            lstContainer.Items.RemoveAt(lstContainer.SelectedIndex);
                            break;
                        case "metal":
                            craftingMaterials[1] += 1;
                            lblMetalCount.Content = craftingMaterials[1];
                            lstContainer.Items.RemoveAt(lstContainer.SelectedIndex);
                            break;
                        case "wood":
                            craftingMaterials[2] += 1;
                            lblWoodCount.Content = craftingMaterials[2];
                            lstContainer.Items.RemoveAt(lstContainer.SelectedIndex);
                            break;
                        case "rope":
                            craftingMaterials[3] += 1;
                            lblRopeCount.Content = craftingMaterials[3];
                            lstContainer.Items.RemoveAt(lstContainer.SelectedIndex);
                            break;
                        case "lighter":
                            craftingMaterials[4] += 1;
                            lblLighterCount.Content = craftingMaterials[4];
                            lstContainer.Items.RemoveAt(lstContainer.SelectedIndex);
                            break;
                        case "duct tape":
                            craftingMaterials[5] += 1;
                            lblDuctTapeCount.Content = craftingMaterials[5];
                            lstContainer.Items.RemoveAt(lstContainer.SelectedIndex);
                            break;
                        case "plastic":
                            craftingMaterials[6] += 1;
                            lblPlasticCount.Content = craftingMaterials[6];
                            lstContainer.Items.RemoveAt(lstContainer.SelectedIndex);
                            break;
                    }
                }
            }
        }
        #endregion

        #region handle Combat

        private void DoZombieAttack()
        {
            if (ReturnRandom(1, 4) == 1)
            {
                lblOutPut.Content += "\nThe zombie did missed it's attack!";
            }
            else
            {
                int zombieDamage = zombieLvl + (ReturnRandom(-1, 1));
                playerHP -= zombieDamage;
                if (playerHP < 0)
                {
                    lblOutPut.Content = "You died!";
                    PlayerDied();
                }
                lblHp.Content = playerHP;
                lblOutPut.Content += "\nThe zombie did " + zombieDamage + " damage!";
            }
        }

        private void PlayerDied()
        {
            btnStab.Visibility = Visibility.Hidden;
            btnShoot.Visibility = Visibility.Hidden;
            btnTnt.Visibility = Visibility.Hidden;
            lblDisplayHp.Visibility = Visibility.Hidden;
            lblDisplayLvl.Visibility = Visibility.Hidden;
            lblEnemyLevel.Visibility = Visibility.Hidden;
            lblEnemyHp.Visibility = Visibility.Hidden;
            lstCommands.Items.Clear();
            //lstCommands.Items.Add("Restart");
        }

        private void BtnStab_Click(object sender, RoutedEventArgs e)
        {
            if (ReturnRandom(1, 5) == 1)
            {
                lblOutPut.Content = "You missed your attack!";
                DoZombieAttack();
            }
            else
            {
                int damage = (level + knifeLvl + 1) * ReturnRandom(1, 3);
                DamageZombie(damage);
                DoZombieAttack();
            }
        }

        // TEST THIS METHOD FOR ERRORS
        private void BtnShoot_Click(object sender, RoutedEventArgs e)
        {
            if (gunLvl == 0)
            {
                lblOutPut.Content = "You don't have a gun!";
            }
            else
            {
                if (bulletAmount == 0)
                {
                    lblOutPut.Content = "You don't have any bullets!";
                }
                else
                {
                    if (ReturnRandom(1, 10) == 1)
                    {
                        lblOutPut.Content = "You missed your attack!";
                        DoZombieAttack();
                    }
                    else
                    {
                        bulletAmount -= 1;
                        lblBulletsCount.Content = bulletAmount;
                        int damage = (level + knifeLvl + 5) * ReturnRandom(1, 3);
                        DamageZombie(damage);
                        DoZombieAttack();
                    }
                }
            }
        }

        // TEST THIS METHOD FOR ERRORS
        private void BtnTnt_Click(object sender, RoutedEventArgs e)
        {
            if (tntAmount == 0)
            {
                lblOutPut.Content = "You don't have any TNT!";
            }
            else
            {
                tntAmount -= 1;
                lblTntCount.Content = tntAmount;
                DamageZombie(100);
                DoZombieAttack();
            }
        }

        // TEST THIS METHOD FOR ERRORS
        private void DamageZombie(int damage)
        {
            zombieHp -= damage;
            lblEnemyHp.Content = zombieHp;
            if (zombieHp <= 0)
            {
                btnStab.Visibility = Visibility.Hidden;
                btnShoot.Visibility = Visibility.Hidden;
                btnTnt.Visibility = Visibility.Hidden;
                lblDisplayHp.Visibility = Visibility.Hidden;
                lblDisplayLvl.Visibility = Visibility.Hidden;
                lblEnemyLevel.Visibility = Visibility.Hidden;
                lblEnemyHp.Visibility = Visibility.Hidden;
                int xpEarned;
                if (level > 5)
                {
                    xpEarned = zombieLvl * ReturnRandom(level - 2, level + 1);
                }
                else
                {
                    xpEarned = zombieLvl * ReturnRandom(1, level + 1);
                }

                XP += xpEarned;
                if (XP >= 100)
                {
                    level += 1;
                    XP -= 100;
                    lblLvl.Content = level;
                }
                lblXp.Content = XP;
                lblOutPut.Content = "You killed the zombie!\nYou gained " + xpEarned + " XP!";
                FillCmdsBack(room);
                OpenContainer("zombie");
            }
            else
            {
                lblOutPut.Content = "You dealt " + damage + " damage to the zombie!";
            }
        }

        // TEST THIS METHOD FOR ERRORS
        private void BtnEatFood_Click(object sender, RoutedEventArgs e)
        {
            if (foodAmount == 0)
            {
                lblOutPut.Content = "You don't have any food!";
            }
            else
            {
                foodAmount -= 1;
                lblFoodCount.Content = foodAmount;
                int HPRecovered = ReturnRandom(35, 50);
                playerHP += HPRecovered;
                if (playerHP > 100)
                {
                    int recoveredHP = 100 - (playerHP);
                    lblOutPut.Content = "You ate some food and recovered " + recoveredHP + " HP!";
                    playerHP = 100;
                }
                else
                {
                    lblOutPut.Content = "You ate some food and recovered " + HPRecovered + " HP!";
                }
            }
            lblHp.Content = playerHP;
        }

        #endregion
    }
}