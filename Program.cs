using System;
using System.IO;
using System.Net;
using System.Text;

public class TebexApi
{
    private const string API_KEY = "TuClaveAPI"; // Reemplaza "TuClaveAPI" con tu clave de API de Tebex.

    public static void Main(string[] args)
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("Menú:");
            Console.WriteLine("1. Generar cupón");
            Console.WriteLine("2. Generar tarjeta de regalo");
            Console.WriteLine("3. Salir");
            Console.Write("Elija una opción: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    GenerateCoupon();
                    break;
                case "2":
                    GenerateGiftCard();
                    break;
                case "3":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Opción no válida. Por favor, elija una opción válida.");
                    break;
            }
        }
    }

    public static void GenerateCoupon()
    {
        Console.Write("Porcentaje de descuento (entero): ");
        if (int.TryParse(Console.ReadLine(), out int discountPercentage))
        {
            Console.Write("Días de validez (entero): ");
            if (int.TryParse(Console.ReadLine(), out int validDays))
            {
                string couponCode = GenerateCouponCode(discountPercentage, validDays);
                if (!string.IsNullOrEmpty(couponCode))
                {
                    Console.WriteLine("Cupón Generado: " + couponCode);
                    AddWatermark(couponCode);
                }
                else
                {
                    Console.WriteLine("No se pudo generar el cupón.");
                }
            }
            else
            {
                Console.WriteLine("Número de días no válido.");
            }
        }
        else
        {
            Console.WriteLine("Porcentaje de descuento no válido.");
        }
    }

    public static void GenerateGiftCard()
    {
        Console.Write("Valor de la tarjeta de regalo (entero): ");
        if (int.TryParse(Console.ReadLine(), out int value))
        {
            string giftCardCode = GenerateGiftCardCode(value);
            if (!string.IsNullOrEmpty(giftCardCode))
            {
                Console.WriteLine("Código de tarjeta de regalo generado: " + giftCardCode);
                AddWatermark(giftCardCode);
            }
            else
            {
                Console.WriteLine("No se pudo generar la tarjeta de regalo.");
            }
        }
        else
        {
            Console.WriteLine("Valor de la tarjeta de regalo no válido.");
        }
    }

    public static string GenerateCouponCode(int discountPercentage, int validDays)
    {
        try
        {
            string urlString = "https://api.tebex.io/v2/coupons";
            string parameters = "code_length=8&discount_percentage=" + discountPercentage + "&expire_never=false&expire_limit=" + validDays;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlString);
            request.Method = "POST";
            request.Headers.Add("X-Tebex-Secret", API_KEY);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = parameters.Length;

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(parameters);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseString = reader.ReadToEnd();
                return responseString;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        return null;
    }

    public static string GenerateGiftCardCode(int value)
    {
        try
        {
            string urlString = "https://api.tebex.io/v2/gift_cards";
            string parameters = "note=Generated gift card&amount=" + value;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlString);
            request.Method = "POST";
            request.Headers.Add("X-Tebex-Secret", API_KEY);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = parameters.Length;

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(parameters);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseString = reader.ReadToEnd();
                return responseString;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        return null;
    }

    public static void AddWatermark(string code)
    {
        string watermark = "Tebex Generator Giftcart & Cupons";
        Console.WriteLine(watermark);
        Console.WriteLine(code);
    }
}