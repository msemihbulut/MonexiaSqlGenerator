using System.Globalization;
using System.Text;

namespace MonexiaSqlGenerator
{
    public enum ExpenseCategory
    {
        Gıda, Ulaşım, Konut, Sağlık, Eğitim, Eğlence, Fatura, Giyim, Tatil, KrediKartıÖdemesi, Abonelikler, Diğer
    }

    public enum IncomeCategory
    {
        Maaş, SerbestGelir, HisseTemettü, KiraGeliri, FaizGeliri, KriptoKazancı, Diğer
    }

    public class Transaction
    {
        public Guid UserId { get; set; }
        public int Type { get; set; } // 0 = Income, 1 = Expense
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public ExpenseCategory? ExpenseCategory { get; set; }
        public IncomeCategory? IncomeCategory { get; set; }
    }

    class Program
    {
        static Random random = new Random();

        static void Main(string[] args)
        {
            var transactions1 = GenerateTransactionsForUser(
                userId: Guid.Parse("7d2d3f7c-4ad7-43af-8840-ba4a1724cf07"),
                totalRecords: 350,
                incomeRatio: 0.65 // örnek
);
            File.WriteAllText("transactions_user1.sql", GenerateSql(transactions1), Encoding.UTF8);

            var transactions2 = GenerateTransactionsForUser(
                userId: Guid.Parse("0e22bbba-4e2a-40a0-a047-00371b98de81"),
                totalRecords: 400,
                incomeRatio: 0.40 // gelir az, gider çok
            );
            File.WriteAllText("transactions_user2.sql", GenerateSql(transactions2), Encoding.UTF8);

            var transactions3 = GenerateTransactionsForUser(
                userId: Guid.Parse("fb9e614f-9b4c-4462-be47-2c792a78809f"),
                totalRecords: 400,
                incomeRatio: 0.55
            );
            File.WriteAllText("transactions_user3.sql", GenerateSql(transactions3), Encoding.UTF8);

            var transactions4 = GenerateTransactionsForUser(
                userId: Guid.Parse("6498459b-acb3-40d4-ac81-8b17579be05c"),
                totalRecords: 500,
                incomeRatio: 0.60
            );
            File.WriteAllText("transactions_user4.sql", GenerateSql(transactions4), Encoding.UTF8);

            var transactions5 = GenerateTransactionsForUser(
                userId: Guid.Parse("fde9a7dc-b079-4b45-933e-628e33702bdf"),
                totalRecords: 450,
                incomeRatio: 0.44
            );
            File.WriteAllText("transactions_user5.sql", GenerateSql(transactions5), Encoding.UTF8);

            var transactions6 = GenerateTransactionsForUser(
                userId: Guid.Parse("b94e3d7e-ff1a-4f87-955a-36c153506ed3"),
                totalRecords: 430,
                incomeRatio: 0.37
            );
            File.WriteAllText("transactions_user6.sql", GenerateSql(transactions6), Encoding.UTF8);

            var transactions7 = GenerateTransactionsForUser(
                userId: Guid.Parse("e4917dcd-cfde-4a00-9449-1563e1c53b4a"),
                totalRecords: 470,
                incomeRatio: 0.68
            );
            File.WriteAllText("transactions_user7.sql", GenerateSql(transactions7), Encoding.UTF8);

            var transactions8 = GenerateTransactionsForUser(
                userId: Guid.Parse("05abe038-0c9f-4d2f-8014-a6f2caaf23e7"),
                totalRecords: 440,
                incomeRatio: 0.30
            );
            File.WriteAllText("transactions_user8.sql", GenerateSql(transactions8), Encoding.UTF8);

            var transactions9 = GenerateTransactionsForUser(
                userId: Guid.Parse("7ce9c2db-6430-40ff-85ee-0fbfb33d237f"),
                totalRecords: 420,
                incomeRatio: 0.57
            );
            File.WriteAllText("transactions_user9.sql", GenerateSql(transactions9), Encoding.UTF8);

            var transactions10 = GenerateTransactionsForUser(
                userId: Guid.Parse("e12b3330-73d4-4b06-8a4a-9171c445711b"),
                totalRecords: 350,
                incomeRatio: 0.52
            );
            File.WriteAllText("transactions_user10.sql", GenerateSql(transactions10), Encoding.UTF8);

            Console.WriteLine("Kayıtlar oluşturuldu!");
        }

        static List<Transaction> GenerateTransactionsForUser(Guid userId, int totalRecords, double incomeRatio)
        {
            var transactions = new List<Transaction>();
            int yearCount = 2;
            DateTime startDate = DateTime.Now.AddYears(-yearCount);

            int incomeCount = (int)(totalRecords * incomeRatio);
            int expenseCount = totalRecords - incomeCount;

            // GELİR: Hangi fonksiyon kullanılacak?
            List<Transaction> monthlyIncomes;
            if (incomeRatio < 0.5)
                monthlyIncomes = GenerateMonthlyIncomesMaaş(startDate, yearCount, userId); // Sadece maaş
            else
                monthlyIncomes = GenerateMonthlyIncomesMaaşKira(startDate, yearCount, userId); // Maaş + kira

            var monthlyExpenses = GenerateMonthlyExpensesCustom(startDate, yearCount, userId, 3);

            // Geriye kalan gelir/gider adedini hesapla
            int otherIncomeCount = Math.Max(0, incomeCount - monthlyIncomes.Count);
            int otherExpenseCount = Math.Max(0, expenseCount - monthlyExpenses.Count);

            var otherIncomes = GenerateOtherIncomes(otherIncomeCount, startDate, yearCount, userId);
            var otherExpenses = GenerateOtherExpenses(otherExpenseCount, startDate, yearCount, userId);

            transactions.AddRange(monthlyIncomes);
            transactions.AddRange(otherIncomes);
            transactions.AddRange(monthlyExpenses);
            transactions.AddRange(otherExpenses);

            // Toplam kayıt sayısı fazla ise random kayıt sil (çok nadiren olabilir)
            while (transactions.Count > totalRecords)
            {
                var idx = random.Next(transactions.Count);
                transactions.RemoveAt(idx);
            }

            transactions = transactions.OrderBy(x => x.TransactionDate).ToList();
            return transactions;
        }

        // Sadece maaş geliri
        static List<Transaction> GenerateMonthlyIncomesMaaş(DateTime startDate, int yearCount, Guid userId)
        {
            var list = new List<Transaction>();
            for (int i = 0; i < 12 * yearCount; i++)
            {
                var date = startDate.AddMonths(i).AddDays(random.Next(0, 5));
                var monthName = CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat.GetMonthName(date.Month);

                list.Add(new Transaction
                {
                    UserId = userId,
                    Type = 0,
                    Amount = RandomDecimal(30000, 50000),
                    TransactionDate = date,
                    Description = $"{monthName} ayı maaş ödemesi",
                    IncomeCategory = IncomeCategory.Maaş
                });
            }
            return list;
        }

        // Maaş + kira geliri
        static List<Transaction> GenerateMonthlyIncomesMaaşKira(DateTime startDate, int yearCount, Guid userId)
        {
            var list = new List<Transaction>();
            for (int i = 0; i < 12 * yearCount; i++)
            {
                var date = startDate.AddMonths(i).AddDays(random.Next(0, 5));
                var monthName = CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat.GetMonthName(date.Month);

                list.Add(new Transaction
                {
                    UserId = userId,
                    Type = 0,
                    Amount = RandomDecimal(30000, 50000),
                    TransactionDate = date,
                    Description = $"{monthName} ayı maaş ödemesi",
                    IncomeCategory = IncomeCategory.Maaş
                });

                list.Add(new Transaction
                {
                    UserId = userId,
                    Type = 0,
                    Amount = RandomDecimal(15000, 20000),
                    TransactionDate = date.AddDays(2),
                    Description = $"Tuzla evi kira geliri ({monthName})",
                    IncomeCategory = IncomeCategory.KiraGeliri
                });
            }
            return list;
        }

        static List<Transaction> GenerateMonthlyExpensesCustom(DateTime startDate, int yearCount, Guid userId, int repeatsPerMonth = 3)
        {
            var list = new List<Transaction>();
            for (int i = 0; i < 12 * yearCount; i++)
            {
                var date = startDate.AddMonths(i).AddDays(random.Next(0, 5));
                var monthName = CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat.GetMonthName(date.Month);

                list.Add(new Transaction
                {
                    UserId = userId,
                    Type = 1,
                    Amount = RandomDecimal(1000, 3000),
                    TransactionDate = date,
                    Description = $"{monthName} faturası",
                    ExpenseCategory = ExpenseCategory.Fatura
                });

                list.Add(new Transaction
                {
                    UserId = userId,
                    Type = 1,
                    Amount = RandomDecimal(2000, 12000),
                    TransactionDate = date.AddDays(2),
                    Description = $"Kredi kartı ekstresi ödemesi",
                    ExpenseCategory = ExpenseCategory.KrediKartıÖdemesi
                });

                list.Add(new Transaction
                {
                    UserId = userId,
                    Type = 1,
                    Amount = RandomDecimal(100, 400),
                    TransactionDate = date.AddDays(5),
                    Description = $"Netflix abonelik ücreti",
                    ExpenseCategory = ExpenseCategory.Abonelikler
                });

                // Ekstra bir gider daha
                if (repeatsPerMonth > 3)
                {
                    list.Add(new Transaction
                    {
                        UserId = userId,
                        Type = 1,
                        Amount = RandomDecimal(1000, 5000),
                        TransactionDate = date.AddDays(7),
                        Description = $"Aylık ek gider",
                        ExpenseCategory = ExpenseCategory.Diğer
                    });
                }
            }
            return list;
        }

        static List<Transaction> GenerateOtherIncomes(int adet, DateTime startDate, int yearCount, Guid userId)
        {
            var list = new List<Transaction>();
            var options = new List<(IncomeCategory, decimal, decimal, string)>
            {
                (IncomeCategory.SerbestGelir, 10000, 15000, "Serbest meslek geliri"),
                (IncomeCategory.HisseTemettü, 5000, 7000, "{0} hisse temettü ödemesi"),
                (IncomeCategory.FaizGeliri, 16000, 18000, "Banka faiz geliri"),
                (IncomeCategory.KriptoKazancı, 2000, 3000, "Bitcoin geliri"),
                (IncomeCategory.Diğer, 5000, 10000, "Diğer gelir")
            };
            for (int i = 0; i < adet; i++)
            {
                var item = options[random.Next(options.Count)];
                var date = RandomDateInPeriod(startDate, yearCount);
                string desc = item.Item4.Contains("{0}")
                    ? string.Format(item.Item4, CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat.GetMonthName(date.Month))
                    : item.Item4;
                list.Add(new Transaction
                {
                    UserId = userId,
                    Type = 0,
                    Amount = RandomDecimal(item.Item2, item.Item3),
                    TransactionDate = date,
                    Description = desc,
                    IncomeCategory = item.Item1
                });
            }
            return list;
        }

        static List<Transaction> GenerateOtherExpenses(int adet, DateTime startDate, int yearCount, Guid userId)
        {
            var list = new List<Transaction>();
            var options = new List<(ExpenseCategory, decimal, decimal, string[])>
            {
                (ExpenseCategory.Gıda, 500, 2000, new[] { "Market harcaması", "Bakkal alışverişi" }),
                (ExpenseCategory.Ulaşım, 100, 200, new[] { "Toplu taşıma", "Taksi ücreti" }),
                (ExpenseCategory.Konut, 1000, 2000, new[] { "Aidat ödemesi", "Ev tadilatı" }),
                (ExpenseCategory.Sağlık, 1000, 2000, new[] { "Eczane harcaması", "Doktor ücreti" }),
                (ExpenseCategory.Eğitim, 0, 1000, new[] { "Online eğitim ücreti", "Seminer kaydı" }),
                (ExpenseCategory.Eğlence, 1000, 3000, new[] { "Sinema harcaması", "Konser bileti" }),
                (ExpenseCategory.Giyim, 1500, 3000, new[] { "Kıyafet alışverişi", "Ayakkabı alımı" }),
                (ExpenseCategory.Tatil, 0, 10000, new[] { "Yaz tatili harcaması", "Otelde konaklama" }),
                (ExpenseCategory.Diğer, 0, 1000, new[] { "Diğer harcama", "Beklenmeyen gider" })
            };

            for (int i = 0; i < adet; i++)
            {
                var item = options[random.Next(options.Count)];
                var date = RandomDateInPeriod(startDate, yearCount);
                var desc = item.Item4[random.Next(item.Item4.Length)];
                list.Add(new Transaction
                {
                    UserId = userId,
                    Type = 1,
                    Amount = RandomDecimal(item.Item2, item.Item3),
                    TransactionDate = date,
                    Description = desc,
                    ExpenseCategory = item.Item1
                });
            }
            return list;
        }

        static decimal RandomDecimal(decimal min, decimal max)
        {
            return Math.Round((decimal)(random.NextDouble() * ((double)max - (double)min) + (double)min), 2);
        }

        static DateTime RandomDateInPeriod(DateTime start, int yearCount)
        {
            int totalDays = (int)(DateTime.Now - start).TotalDays;
            return start.AddDays(random.Next(totalDays)).Date
                .AddHours(random.Next(0, 23))
                .AddMinutes(random.Next(0, 59));
        }

        static string GenerateSql(List<Transaction> transactions)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var t in transactions)
            {
                string expense = t.ExpenseCategory.HasValue ? ((int)t.ExpenseCategory.Value).ToString() : "NULL";
                string income = t.IncomeCategory.HasValue ? ((int)t.IncomeCategory.Value).ToString() : "NULL";
                sb.AppendLine(
                    $"INSERT INTO [Monexia].[dbo].[Transactions] ([UserId], [Type], [Amount], [TransactionDate], [Description], [ExpenseCategory], [IncomeCategory]) VALUES " +
                    $"('{t.UserId}', {t.Type}, {t.Amount.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}, '{t.TransactionDate:yyyy-MM-dd HH:mm:ss}', '{t.Description.Replace("'", "''")}', {expense}, {income});"
                );
            }
            return sb.ToString();
        }
    }
}
