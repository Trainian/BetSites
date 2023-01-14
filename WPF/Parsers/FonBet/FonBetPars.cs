using ApplicationCore.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Parsers.Base;
using WPF.Static;

namespace WPF.Parsers.FonBet
{
    public class FonBetPars : BetPars
    {
        private Random _random;
        private List<Bet> bets;
        private FonBetDebt debtService;
        private FonBetModel modelservice;

        public FonBetPars()
        {
            debtService = new FonBetDebt();
            modelservice = new FonBetModel();
        }

        public override async Task<ICollection<Bet>> ParsSite(IWebDriver driver, bool betIsDisabled)
        {
            bets = new List<Bet>();
            _random = new Random(DateTime.Now.Second);

            IReadOnlyCollection<IWebElement> sports = new List<IWebElement>();
            sports = driver.FindElements(By.CssSelector(SearchElements.Bet));

            if (sports.Count == 0)
                return bets;

            foreach (var bet in sports)
            {
                try
                {
                    var name = bet.FindElement(By.CssSelector(SearchElements.BetName)).Text;
                    if (name.Contains("—"))
                    {
                        var newBet = modelservice.CreateModels(bet);
                        var debt = await debtService.CheckAndDebt(bet, newBet);

                        if (debt == true && !betIsDisabled)
                        {
                            var modalWindowFastBet = driver.FindElement(By.CssSelector(SearchElements.FastBetModel));
                            if (modalWindowFastBet != null)
                            {
                                driver.FindElement(By.CssSelector(SearchElements.FastBetModalYes)).Click();
                            }
                        }

                        if (debt == true)
                        {
                            newBet.Coefficients.Last().IsMadeBet = true;
                            await Task.Delay(_random.Next(1500, 5000));
                        }
                        bets.Add(newBet);
                    };
                }
                catch (StaleElementReferenceException e) { Debug.WriteLine($"------------> Элемент парсинга, был удален из DOM \n{e.Message}"); }
                catch (Exception e) { Debug.WriteLine($"!-!-!-!-!-!-> Критическое ислючение, вызвано: \n{e.Message}"); }
            }
            return bets;
        }
    }
}
