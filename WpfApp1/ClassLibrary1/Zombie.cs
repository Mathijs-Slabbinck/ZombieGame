namespace ClassLibrary1
{
    public class Zombie
    {
        private Random rand = new Random();
        private int hp = 100;
        private int lvl = 0;
        private bool hasWeapon = false;
        private int baseDmg = 1;

        public Zombie(int playerLvl)
        {
            this.hp = 100;
            SetLvlBasedOnPlayerLvl(playerLvl);
            SetHasWeapon();
            SetBaseDmg();
        }

        public int Hp
        {
            get { return hp; }
        }

        public int Lvl
        {
            get { return lvl; }
            private set // made it private so you can only set it using SetLvlBasedOnPlayerLvl()
            {
                if (value >= 0)
                {
                    lvl = value;
                }
                else
                {
                    lvl = 0;
                } 
            }
        }

        public bool HasWeapon
        {
            get { return hasWeapon; }
        }

        public int BaseDmg
        {
            get { return baseDmg; }
        }

        private void SetHasWeapon()
        {
            int oneOrZero = rand.Next(0, 2);

            switch (oneOrZero)
            {
                case 0:
                    hasWeapon = false;
                    break;

                case 1:
                    hasWeapon = true;
                    break;
            }
        }

        private void SetLvlBasedOnPlayerLvl(int playerLvl)
        {
            if(playerLvl >= 6)
            {
                Lvl = playerLvl + rand.Next(-5, 4); // playerlevel -5 of +3
            }
            else
            {
                Lvl = playerLvl + rand.Next(0, 4); // playerlevel +0 of +3
            }
        }

        public void DealDamage(int damage)
        {
            hp -= damage;
            if(hp <= 0)
            {
                hp = 0;
            }
        }

        private void SetBaseDmg()
        {
            baseDmg = rand.Next(1, 6); // getal tussen 1 en 5
        }
    }
}
