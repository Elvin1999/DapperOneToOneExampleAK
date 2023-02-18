using Dapper;
using DapperOneToOneExample.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace DapperOneToOneExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //LoadCapitals();
           // LoadCountries();
        }

        public async void LoadCapitals()
        {

            var conn = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

            using (var connection = new SqlConnection(conn))
            {
                var sql = @"SELECT CA.CapitalId,CA.Name,CA.CountryId,
                        CO.CountryId,CO.Name
                        FROM Capitals AS CA
                        INNER JOIN Countries AS CO
                        ON CA.CountryId=CO.CountryId";

                var result = await connection.QueryAsync<Capital, Country, Capital>(sql,
                    (capital, country) =>
                    {
                        capital.Country = country;
                        return capital;
                    }, splitOn: "CountryId");
                var capitals = result.ToList();
                myDataGrid.ItemsSource = capitals;

            }
        }
        public async void LoadCountries()
        {

            var conn = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;

            using (var connection = new SqlConnection(conn))
            {
                var sql = @"SELECT CO.CountryId,CO.Name,
                            CA.CapitalId,CA.Name,CA.CountryId
                            FROM Countries AS CO
                            INNER JOIN Capitals AS CA
                            ON CO.CountryId=CA.CountryId";

                var result = await connection.QueryAsync<Country, Capital, Country>(sql,
                    (country, capital) =>
                    {
                        country.Capital = capital;
                        country.CapitalId = capital.CapitalId;
                        return country;
                    }, splitOn: "CapitalId");
                var countries = result.ToList();
                myDataGrid.ItemsSource = countries;

            }
        }
    }
}
