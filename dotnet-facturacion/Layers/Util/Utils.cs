using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public static class Utils
{
    public static void CultureInfo()
    {
        // Colocar Cultura Estandar para Costa Rica
        // esto me permite no tener problemas con (.) de los decimales
        CultureInfo Micultura = new CultureInfo("es-CR", false);
        Micultura.NumberFormat.CurrencySymbol = "¢";
        Micultura.NumberFormat.CurrencyDecimalDigits = 2;
        Micultura.NumberFormat.CurrencyDecimalSeparator = ".";
        Micultura.NumberFormat.CurrencyGroupSeparator = ",";
        int[] grupo = new int[] { 3, 3, 3 };
        Micultura.NumberFormat.CurrencyGroupSizes = grupo;
        Micultura.NumberFormat.NumberDecimalDigits = 2;
        Micultura.NumberFormat.NumberGroupSeparator = ",";
        Micultura.NumberFormat.NumberDecimalSeparator = ".";
        Micultura.NumberFormat.NumberGroupSizes = grupo;
        //Micultura.DateTimeFormat.
        Thread.CurrentThread.CurrentCulture = Micultura;
    }
    
}

