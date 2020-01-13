using GutenBank.Domain;
using GutenBank.Exceptions;
using GutenBank.Models;
using GutenBank.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GutenBank.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("balance")]
        public async Task<ActionResult<AccountDTO>> GetBalance(int accountNumber)
        {
            try
            {
                var account = await _accountService.GetAccount(accountNumber);

                return account;
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Account);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = new Message().ToString() });
            }
        }

        [HttpPost("deposit")]
        public async Task<ActionResult<AccountDTO>> PostDeposit(TransactionDTO transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.Select(v => v.Errors) });
            }

            var isValidCurrency = Enum.TryParse(transaction.Currency, out Currency currency);

            if (!isValidCurrency)
            {
                return BadRequest(new { error = "Invalid Currency" });
            }


            try
            {
                var account = await _accountService.Deposit(transaction);

                return account;
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Account);
            }
            catch (UpdateNotAllowedException ex)
            {
                return Unauthorized(ex.Account);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = new Message().ToString() });
            }

        }

        [HttpPost("withdraw")]
        public async Task<ActionResult<AccountDTO>> PostWithdraw(TransactionDTO transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var isValidCurrency = Enum.TryParse(transaction.Currency, out Currency currency);

            if (!isValidCurrency)
            {
                return BadRequest(new { error = "Invalid Currency" });
            }


            try
            {
                var account = await _accountService.Withdraw(transaction);

                return account;
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Account);
            }
            catch (InsufficientBalanceException ex)
            {
                return BadRequest(ex.Account);
            }
            catch (UpdateNotAllowedException ex)
            {
                return Unauthorized(ex.Account);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = new Message().ToString() });
            }
        }
    }
}
