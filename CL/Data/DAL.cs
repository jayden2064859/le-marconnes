using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CL.Models;

namespace CL.Data
{
    public static class DAL
    {
        public static string connectionString = @"Server=MSI\SQLEXPRESS;Database=LeMarconnesDB;Trusted_Connection=True;TrustServerCertificate=True;";

        // reservering toevoegen
        public static bool InsertReservering(Reservering reservering)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                INSERT INTO Reserveringen 
                    (KlantId, AccommodatieId, StartDatum, EindDatum, AantalVolwassenen, 
                     AantalKinderen0_7, AantalKinderen7_12, AantalHonden, HeeftElectriciteit, 
                     AantalDagenElectriciteit, TotaalPrijs, RegistratieDatum)

                VALUES (@KlantId, @AccommodatieId, @StartDatum, @EindDatum, @AantalVolwassenen,
                            @AantalKinderen0_7, @AantalKinderen7_12, @AantalHonden, @HeeftElectriciteit,
                            @AantalDagenElectriciteit, @TotaalPrijs, @RegistratieDatum);

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
                    command.Parameters.AddWithValue("@AantalHonden", reservering.AantalHonden);
                    command.Parameters.AddWithValue("@HeeftElectriciteit", reservering.HeeftElectriciteit);

                    command.Parameters.AddWithValue("@AantalDagenEleCtriciteit", reservering.AantalDagenElectriciteit);
                    command.Parameters.AddWithValue("@TotaalPrijs", reservering.TotaalPrijs);
                    command.Parameters.AddWithValue("@RegistratieDatum", reservering.RegistratieDatum);
                    command.Parameters.AddWithValue("@Status", reservering.Status);


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
                            ReserveringId = Convert.ToInt32(reader["ReserveringId"]),
                            KlantId = Convert.ToInt32(reader["KlantId"]),
                            AccommodatieId = Convert.ToInt32(reader["AccommodatieId"]),
                            StartDatum = Convert.ToDateTime(reader["StartDatum"]),
                            EindDatum = Convert.ToDateTime(reader["EindDatum"]),
                            AantalVolwassenen = Convert.ToInt32(reader["AantalVolwassenen"]),
                            AantalKinderen0_7 = Convert.ToInt32(reader["AantalKinderen0_7"]),
                            AantalKinderen7_12 = Convert.ToInt32(reader["AantalKinderen7_12"]),
                            AantalHonden = Convert.ToInt32(reader["AantalHonden"]),
                            HeeftElectriciteit = Convert.ToBoolean(reader["HeeftElectriciteit"]),
                            AantalDagenElectriciteit = Convert.ToInt32(reader["AantalDagenElectriciteit"]),
                            TotaalPrijs = Convert.ToDecimal(reader["TotaalPrijs"]),
                            Status = reader["Status"].ToString(),
                            RegistratieDatum = Convert.ToDateTime(reader["RegistratieDatum"])
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

        // reservering ophalen op id
        public static Reservering GetReserveringById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Reserveringen WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        return new Reservering
                        {
                            ReserveringId = Convert.ToInt32(reader["ReserveringId"]),
                            KlantId = Convert.ToInt32(reader["KlantId"]),
                            AccommodatieId = Convert.ToInt32(reader["AccommodatieId"]),
                            StartDatum = Convert.ToDateTime(reader["StartDatum"]),
                            EindDatum = Convert.ToDateTime(reader["EindDatum"]),
                            AantalVolwassenen = Convert.ToInt32(reader["AantalVolwassenen"]),
                            AantalKinderen0_7 = Convert.ToInt32(reader["AantalKinderen0_7"]),
                            AantalKinderen7_12 = Convert.ToInt32(reader["AantalKinderen7_12"]),
                            AantalHonden = Convert.ToInt32(reader["AantalHonden"]),
                            HeeftElectriciteit = Convert.ToBoolean(reader["HeeftElectriciteit"]),
                            AantalDagenElectriciteit = Convert.ToInt32(reader["AantalDagenElectriciteit"]),
                            TotaalPrijs = Convert.ToDecimal(reader["TotaalPrijs"]),
                            Status = reader["Status"].ToString(),
                            RegistratieDatum = Convert.ToDateTime(reader["RegistratieDatum"])
                        };
                    }
                }
            }
        }


        // reservering updaten gebaseerd op id
        public static bool UpdateReservering(Reservering res)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = @"
            UPDATE Reserveringen SET
                KlantId = @KlantId,
                AccommodatieId = @AccommodatieId,
                StartDatum = @StartDatum,
                EindDatum = @EindDatum,
                AantalVolwassenen = @AantalVolwassenen,
                AantalKinderen0_7 = @AantalKinderen0_7,
                AantalKinderen7_12 = @AantalKinderen7_12,
                AantalHonden = @AantalHonden,
                HeeftElectriciteit = @HeeftElectriciteit,
                AantalDagenElectriciteit = @AantalDagenElectriciteit,
                Status = @Status
            WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ReserveringId", res.ReserveringId);
                    command.Parameters.AddWithValue("@KlantId", res.KlantId);
                    command.Parameters.AddWithValue("@AccommodatieId", res.AccommodatieId);
                    command.Parameters.AddWithValue("@StartDatum", res.StartDatum);
                    command.Parameters.AddWithValue("@EindDatum", res.EindDatum);
                    command.Parameters.AddWithValue("@AantalVolwassenen", res.AantalVolwassenen);
                    command.Parameters.AddWithValue("@AantalKinderen0_7", res.AantalKinderen0_7);
                    command.Parameters.AddWithValue("@AantalKinderen7_12", res.AantalKinderen7_12);
                    command.Parameters.AddWithValue("@AantalHonden", res.AantalHonden);
                    command.Parameters.AddWithValue("@HeeftElectriciteit", res.HeeftElectriciteit);
                    command.Parameters.AddWithValue("@AantalDagenElectriciteit", res.AantalDagenElectriciteit);
                    command.Parameters.AddWithValue("@Status", res.Status);

                    int rows = command.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public static List<Tarief> TarievenOphalen()
        {
            List<Tarief> tarieven = new List<Tarief>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Tarieven WHERE AccommodatieTypeId = 1"; // alleen camping tarieven worden opgehaald

                using (SqlCommand command = new SqlCommand(sql, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tarieven.Add(new Tarief
                        {
                            TariefId = Convert.ToInt32(reader["Id"]),
                            AccommodatieTypeId = Convert.ToInt32(reader["AccommodatieTypeId"]),
                            Type = reader["Type"].ToString(),
                            Prijs = Convert.ToDecimal(reader["Prijs"])
                        });
                    }
                }
            }
            return tarieven;
        }

    }
}