using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CL.Models;

namespace CL.Data
{
    public static class DAL
    {
        public static string connectionString = @"Server=localhost\SQLEXPRESS;Database=LeMarconnesDB;Trusted_Connection=True;TrustServerCertificate=True;";

        // reservering toevoegen
        public static bool InsertReservering(Reservering reservering)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    INSERT INTO Reserveringen 
                    (KlantId, AccommodatieId, StartDatum, EindDatum, AantalVolwassenen, 
                     AantalKinderen0_7, AantalKinderen7_12, Hond, Elektriciteit, 
                     AantalDagenElektriciteit, TotaalPrijs, DatumAangemaakt) 
                    VALUES (@KlantId, @AccommodatieId, @StartDatum, @EindDatum, @AantalVolwassenen,
                            @AantalKinderen0_7, @AantalKinderen7_12, @Hond, @Elektriciteit,
                            @AantalDagenElektriciteit, @TotaalPrijs, @DatumAangemaakt);
                ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@KlantId", reservering.KlantId);
                    command.Parameters.AddWithValue("@AccommodatieId", reservering.AccommodatieId);
                    command.Parameters.AddWithValue("@StartDatum", reservering.StartDatum);
                    command.Parameters.AddWithValue("@EindDatum", reservering.EindDatum);
                    command.Parameters.AddWithValue("@AantalVolwassenen", reservering.AantalVolwassenen);
                    command.Parameters.AddWithValue("@AantalKinderen0_7", reservering.AantalKinderen0_7);
                    command.Parameters.AddWithValue("@AantalKinderen7_12", reservering.AantalKinderen7_12);
                    command.Parameters.AddWithValue("@Hond", reservering.Hond);
                    command.Parameters.AddWithValue("@Elektriciteit", reservering.Elektriciteit);
                    command.Parameters.AddWithValue("@AantalDagenElektriciteit", reservering.AantalDagenElektriciteit);
                    command.Parameters.AddWithValue("@TotaalPrijs", reservering.TotaalPrijs);
                    command.Parameters.AddWithValue("@DatumAangemaakt", reservering.DatumAangemaakt);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // alle reserveringen ophalen
        public static List<Reservering> GetAllReserveringen()
        {
            List<Reservering> reserveringen = new List<Reservering>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Reserveringen";

                using (SqlCommand command = new SqlCommand(sql, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Reservering reservering = new Reservering
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            KlantId = Convert.ToInt32(reader["KlantId"]),
                            AccommodatieId = Convert.ToInt32(reader["AccommodatieId"]),
                            StartDatum = Convert.ToDateTime(reader["StartDatum"]),
                            EindDatum = Convert.ToDateTime(reader["EindDatum"]),
                            AantalVolwassenen = Convert.ToInt32(reader["AantalVolwassenen"]),
                            AantalKinderen0_7 = Convert.ToInt32(reader["AantalKinderen0_7"]),
                            AantalKinderen7_12 = Convert.ToInt32(reader["AantalKinderen7_12"]),
                            Hond = Convert.ToBoolean(reader["Hond"]),
                            Elektriciteit = Convert.ToBoolean(reader["Elektriciteit"]),
                            AantalDagenElektriciteit = Convert.ToInt32(reader["AantalDagenElektriciteit"]),
                            TotaalPrijs = Convert.ToDecimal(reader["TotaalPrijs"]),
                            Status = reader["Status"].ToString(),
                            DatumAangemaakt = Convert.ToDateTime(reader["DatumAangemaakt"])
                        };
                        reserveringen.Add(reservering);
                    }
                }
            }

            return reserveringen;
        }

        // reservering verwijderen op id
        public static bool DeleteReservering(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "DELETE FROM Reserveringen WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}