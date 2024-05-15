using System;
using System.Security.Principal;
using ebanx_api.AccountRecords;

namespace ebanx_api
{
	public static class Services
	{
		private static List<AccountRecords.Account> Accounts = new List<AccountRecords.Account>();

		internal static void ResetAccounts()
		{
			Accounts = new List<AccountRecords.Account>();
		}

		internal static AccountRecords.Account? GetAccount(string id)
		{
			return Accounts.Find(x => x.Id == id);
		}

		internal static AccountReturn UpsertAccountAndBalance(AccountRecords.AccountDto account)
		{
            var origin = account.Origin;
			var destination = account.Destination;
			var amount = account.Amount;

            switch (account.Type)
            {
                case "deposit":
                    return Deposit(destination, amount);
                case "withdraw":
                    return Withdraw(origin, amount);
                case "transfer":
                    return Transfer(origin, destination, amount);
                default:
                    throw new Exception($"Type {account.Type} not found");
            }
        }

		private static AccountReturn Deposit(string id, double amount)
		{
            var dbAccount = Accounts.Find(x => x.Id == id);

            if (dbAccount is null)
            {
                dbAccount = CreateAccount(id, amount);
            }
			else
			{
                dbAccount.Balance += amount;
            }

            var accountReturn = new AccountReturn
            {
                Destination = dbAccount
            };

			return accountReturn;
		}

        private static AccountReturn Withdraw(string id, double amount)
        {
            var dbAccount = Accounts.Find(x => x.Id == id);

            if (dbAccount is null)
            {
                throw new Exception($"Accoung {id} not found");
            }
            else if (dbAccount.Balance < amount)
            {
                throw new Exception($"Insuficient balance");
            }
            else
            {
                dbAccount.Balance -= amount;
            }

            var accountReturn = new AccountReturn
            {
                Origin = dbAccount
            };

            return accountReturn;
        }

        private static AccountReturn Transfer(string origin, string destination, double amount)
        {
            var oAccount = Accounts.Find(x => x.Id == origin) ?? CreateAccount(origin, 0);
            var dAccount = Accounts.Find(x => x.Id == destination) ?? CreateAccount(destination, 0);

            if (oAccount.Balance < amount)
            {
                throw new Exception("Insuficient balance");
            }

            oAccount.Balance -= amount;
            dAccount.Balance += amount;

            var accountReturn = new AccountReturn
            {
                Origin = oAccount,
                Destination = dAccount
            };

            return accountReturn;
        }


        private static Account CreateAccount(string id, double amount)
		{
			var newAccount = new Account
			{
				Balance = amount,
				Id = id
			};

            Accounts.Add(newAccount);

			return newAccount;
        }
    }
}

