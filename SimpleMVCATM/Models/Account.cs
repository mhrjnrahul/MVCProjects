namespace SimpleMVCATM.Models
{
    public class Account
    {
        //properties: what account must have
        public int AccNumber { get; set; }
        public string? AccHolderName { get; set; }
        public int Pin { get; set; }
        public decimal Balance { get; set; }


        //constructor to make a new account
        public Account (int accountNumber, string accHolderName, int pin, decimal balance)
        {
            AccNumber = accountNumber;
            AccHolderName = accHolderName;
            Pin = pin;
            Balance = balance;
        }

        //withdraw menthod
        public bool WithdrawAmount (decimal amount)
        {
            if(amount > Balance)
            {
                //insufficient balance
                return false;
            }

            if(amount < 0)
            {
                //amount must be +ve 
                return false;
            }

            if(amount > 50000)
            {
                return false;
            }

            //withdraw
            Balance -= amount;
            return true;
        }


        //deposit method
        public bool DepositAmount(decimal amount)
        {
            if (amount < 0)
            {
                return false;
            }

            //deposit money
            Balance += amount;
            return true; 
        }
        
    }
}
