using Microsoft.AspNetCore.Mvc;
using SimpleMVCATM.Models;

namespace SimpleMVCATM.Controllers
{
    public class ATMController : Controller
    {
        private static Account currentAccount = new Account(
            accountNumber: 5543,
            accHolderName: "Rahul",
            pin: 4447,
            balance: 20000);

        public IActionResult Index()
        {
            return View(currentAccount);
        }

        public IActionResult Balance()
        {
            return View(currentAccount);
        }

        //withdraw
        [HttpGet]
        public IActionResult Withdraw()
        {
            return View(currentAccount);
        }

        //process the withdraw
        //runs after form
        [HttpPost]
        public IActionResult Withdraw(decimal amount)
        {
            bool success = currentAccount.WithdrawAmount(amount);

            if (success)
            {
                ViewBag.Message = $"Succesffuly withdrawn {amount}. New Balance is: {currentAccount.Balance}";
                ViewBag.IsSuccess = true;
            } else
            {
                ViewBag.Message = "Withdrawal failed";
                ViewBag.IsSuccess = false;
            }
                return View(currentAccount);
        }

        //deposit
        [HttpGet]
        public IActionResult Deposit()
        {
            return View(currentAccount);
        }

        //form wala
        [HttpPost]
        public IActionResult Deposit(decimal amount)
        {
            bool success = currentAccount.DepositAmount(amount);

            if(success)
            {
                ViewBag.Message = $"Succesffuly deposoited {amount}. New Balance is: {currentAccount.Balance}";
                ViewBag.IsSuccess = true;
            } else
            {
                ViewBag.Message = "Depost failed";
                ViewBag.IsSuccess = false;
            }
            return View(currentAccount) ;
        }
        
    }
}
