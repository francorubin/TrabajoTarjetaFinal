using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TarjetaSube
{
    public class Boleto
    {
        private decimal monto;
        private decimal saldoRestante;
        private string lineaColectivo;
        private DateTime fecha;

        public decimal Monto
        {
            get { return monto; }
        }

        public decimal SaldoRestante
        {
            get { return saldoRestante; }
        }

        public string LineaColectivo
        {
            get { return lineaColectivo; }
        }

        public DateTime Fecha
        {
            get { return fecha; }
        }

        public Boleto(decimal monto, decimal saldoRestante, string lineaColectivo)
        {
            this.monto = monto;
            this.saldoRestante = saldoRestante;
            this.lineaColectivo = lineaColectivo;
            this.fecha = DateTime.Now;
        }

        // Constructor adicional para testing con fecha específica
        public Boleto(decimal monto, decimal saldoRestante, string lineaColectivo, DateTime fecha)
        {
            this.monto = monto;
            this.saldoRestante = saldoRestante;
            this.lineaColectivo = lineaColectivo;
            this.fecha = fecha;
        }
    }
}