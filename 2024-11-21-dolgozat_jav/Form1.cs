using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2024_11_21_dolgozat_jav
{
    public partial class Form1 : Form
    {
        Timer walkTimer = new Timer();
        Timer hitTimer = new Timer();
        Timer gravity = new Timer();

        int hitCount = 0;
        int hitDirection = 1; //1 balra, 0 nem mozog, -1 jobbra.
        int hitFrames = 0;
        int hitMaxFrames = 3;
        int requiredHits = 10;
        int apples = 0;
        int storageCapacity = 20;
        int priceOfBiggerStorage = 10;
        int priceOfFasterHit = 30;

        bool holdingAnApple = false;
        //bool facingLeft = true;

        public Form1()
        {
            InitializeComponent();
            alma.SendToBack();

            kosar.BringToFront();
            Start();
        }
        void Start() 
        {

            StartTimers();
            AddEvents();
        }
        void StartTimers()
        {
            walkTimer.Interval = 16;
            hitTimer.Interval = 16;
            gravity.Interval = 16;
            walkTimer.Start();
            walkTimer.Tick += WalkEvent;
            hitTimer.Tick += HitEvent;
            gravity.Tick += GravityEvent;
        }

        void AddEvents() 
        {
            buyBiggerStorage.Click += BuyBiggerStorage;
            buyFasterHit.Click += BuyFasterHit;
        }
        void WalkEvent(object s, EventArgs e)
        {
            //sétálunk
            if (kez.Left > torzs.Right && !holdingAnApple)
            {
                fej.Left -= 3;
                kez.Left -= 3;
                test.Left -= 3;
            }
            //nem sétálunk
            else if (!holdingAnApple)
            {
                walkTimer.Stop();
                hitDirection = 1;
                hitTimer.Start();
            }
            else if (holdingAnApple && test.Right < kosar.Left)
            {
                fej.Left += 3;
                kez.Left += 3;
                test.Left += 3;
                alma.Left += 3;
            }
            // kosárba dobás
            else if (holdingAnApple && test.Right >= kosar.Left)
            {
                walkTimer.Stop();
                gravity.Start();
               
            }
        }

        void HitEvent(object s, EventArgs e)
        {
            if (hitDirection == 1)
            {
                kez.Left -= 6;
                hitFrames++;
                if (hitFrames == hitMaxFrames)
                {
                    hitDirection = -1;
                    hitFrames = 0;
                }
            }
            else if (hitDirection == -1)
            {
                kez.Left += 6;
                hitFrames++;
                if (hitFrames == hitMaxFrames)
                {
                    
                    hitFrames = 0;
                    hitCount++;
                    this.Text = hitCount.ToString();
                    if (hitCount == requiredHits)
                    {
                        hitDirection = 0;
                        hitTimer.Stop();
                        alma.Left = kez.Left;
                        alma.Top = lomb.Bottom - alma.Height;
                        alma.Show();
                        gravity.Start();
                    }
                    else
                    {
                        hitDirection = 1;
                    }
                }
            }

                



        }

        void GravityEvent(object s, EventArgs e)
        {
            //alma zuhan a fáról a kézbe
            if (alma.Bottom < kez.Top && !holdingAnApple)
            {
                alma.Top += 3;
            }
            else if (!holdingAnApple)
            {
                holdingAnApple = true;
                gravity.Stop();
                //forgás jobbra
                kez.Left = test.Left + test.Width / 2;
                alma.Left = kez.Right - alma.Width;
                //forgás vége

                walkTimer.Start();
            }
            else if (holdingAnApple)
            {
                alma.Top += 3;
                if (alma.Top > kosar.Top)
                { 
                    gravity.Stop();
                    holdingAnApple = false;
                    //forgás balra
                    kez.Left = test.Left + test.Width / 2 - kez.Width;
                    // forgás vége

                    hitCount = 0;
                    if (apples < storageCapacity)
                    {
                        apples++;
                    }
                    UpdateAppleLabel();
                    alma.Hide();

                    walkTimer.Start();
                    
                   

                    UpdateAppleLabel();

                }
            }
        }
        void BuyBiggerStorage(object s, EventArgs e) 
        {
            if (apples >= priceOfBiggerStorage) {
                apples -= priceOfBiggerStorage;
                UpdateAppleLabel();
                storageCapacity += 5;
                priceOfBiggerStorage += 2;
                UpdateStorageCapacity();
                buyBiggerStorage.Text = $"{priceOfBiggerStorage} alma";
            }
        }
        void BuyFasterHit(object s, EventArgs e)
        {
            if (apples >= priceOfFasterHit)
            {
                apples -= priceOfFasterHit;
                if (requiredHits > 3) {
                    requiredHits--;
                    priceOfFasterHit += 30;
                    UpdateAppleLabel();
                    buyFasterHit.Text = $"{priceOfFasterHit} alma";
                }
               
                
                UpdateStorageCapacity();
            }
        }

        void UpdateAppleLabel()
        {
            appleCounter.Text = $"Gyűjtött almák száma: {apples}";
        }

        void UpdateStorageCapacity() 
        {
            storageLabel.Text = $"Kosár teherbírása: {storageCapacity} alma";
            //buyBiggerStorage.Text = "";
        }
    }
}