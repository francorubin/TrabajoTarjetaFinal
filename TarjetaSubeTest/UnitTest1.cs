using NUnit.Framework;
using NUnit.Framework.Internal;
using TarjetaSube;

namespace TarjetaSubeTest
{
    [TestFixture]
    public class TarjetaTests
    {
        private Tarjeta tarjeta;

        [SetUp]
        public void Setup()
        {
            tarjeta = new Tarjeta();
        }

        [Test]
        public void TestTarjetaNuevaTieneSaldoCero()
        {
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        [TestCase(2000)]
        [TestCase(3000)]
        [TestCase(4000)]
        [TestCase(5000)]
        [TestCase(8000)]
        [TestCase(10000)]
        [TestCase(15000)]
        [TestCase(20000)]
        [TestCase(25000)]
        [TestCase(30000)]
        public void TestCargasValidas(decimal monto)
        {
            bool resultado = tarjeta.Cargar(monto);
            Assert.IsTrue(resultado);
            Assert.AreEqual(monto, tarjeta.Saldo);
        }

        [Test]
        public void TestCargaInvalida()
        {
            bool resultado = tarjeta.Cargar(1000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestLimiteDeSaldo()
        {
            tarjeta.Cargar(30000);
            tarjeta.Cargar(10000);
            bool resultado = tarjeta.Cargar(2000);
            Assert.IsFalse(resultado);
            Assert.AreEqual(40000, tarjeta.Saldo);
        }

        [Test]
        public void TestPagoConSaldoSuficiente()
        {
            Colectivo colectivo = new Colectivo("K");
            tarjeta.Cargar(5000);

            Boleto? boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(1580, boleto.Monto);
            Assert.AreEqual(3420, tarjeta.Saldo);
            Assert.AreEqual(3420, boleto.SaldoRestante);
        }

        [Test]
        public void TestPagoSinSaldoSuficiente()
        {
            Colectivo colectivo = new Colectivo("K");

            Boleto? boleto = colectivo.PagarCon(tarjeta);

            Assert.IsNull(boleto);
            Assert.AreEqual(0, tarjeta.Saldo);
        }

        [Test]
        public void TestVariosViajes()
        {
            Colectivo colectivo = new Colectivo("143");
            tarjeta.Cargar(5000);

            Boleto? boleto1 = colectivo.PagarCon(tarjeta);
            Boleto? boleto2 = colectivo.PagarCon(tarjeta);
            Boleto? boleto3 = colectivo.PagarCon(tarjeta);

            Assert.IsNotNull(boleto1);
            Assert.IsNotNull(boleto2);
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(260, tarjeta.Saldo);
        }

        [Test]
        public void TestSaldoNegativoPermitido()
        {
            Colectivo colectivo = new Colectivo("K");
            Boleto? boleto = colectivo.PagarCon(tarjeta);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(-1580, tarjeta.Saldo);
        }

        [Test]
        public void TestSaldoNegativoNoSuperaLimite()
        {
            Colectivo colectivo = new Colectivo("K");
            tarjeta.Cargar(2000);
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(-160, tarjeta.Saldo);
            Boleto? boleto3 = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto3);
            Assert.AreEqual(-160, tarjeta.Saldo);
        }

        [Test]
        public void TestRecargaConSaldoNegativoDescuentaViajePlus()
        {
            Colectivo colectivo = new Colectivo("K");
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(-1580, tarjeta.Saldo);
            tarjeta.Cargar(3000);
            Assert.AreEqual(1420, tarjeta.Saldo);
        }

        [Test]
        public void TestMultiplesViajesPlusYRecarga()
        {
            Colectivo colectivo = new Colectivo("K");
            colectivo.PagarCon(tarjeta);
            Assert.AreEqual(-1580, tarjeta.Saldo);
            Boleto? boleto2 = colectivo.PagarCon(tarjeta);
            Assert.IsNull(boleto2);
            Assert.AreEqual(-1580, tarjeta.Saldo);
            tarjeta.Cargar(5000);
            Assert.AreEqual(3420, tarjeta.Saldo);
        }

        [Test]
        public void TestMedioBoletoMontoPagadoEsMitad()
        {
            TarjetaMedioBoleto tarjetaMedio = new TarjetaMedioBoleto();
            tarjetaMedio.Cargar(5000);
            Colectivo colectivo = new Colectivo("K");
            Boleto? boleto = colectivo.PagarCon(tarjetaMedio);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(790, boleto.Monto);
            Assert.AreEqual(4210, tarjetaMedio.Saldo);
        }

        [Test]
        public void TestMedioBoletoMultiplesViajes()
        {
            TarjetaMedioBoleto tarjetaMedio = new TarjetaMedioBoleto();
            tarjetaMedio.Cargar(3000);
            Colectivo colectivo = new Colectivo("143");
            Boleto? boleto1 = colectivo.PagarCon(tarjetaMedio);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(790, boleto1.Monto);
            Boleto? boleto2 = colectivo.PagarCon(tarjetaMedio);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(790, boleto2.Monto);
            Boleto? boleto3 = colectivo.PagarCon(tarjetaMedio);
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(790, boleto3.Monto);
            Assert.AreEqual(630, tarjetaMedio.Saldo);
        }

        [Test]
        public void TestMedioBoletoConSaldoNegativo()
        {
            TarjetaMedioBoleto tarjetaMedio = new TarjetaMedioBoleto();
            Colectivo colectivo = new Colectivo("K");
            Boleto? boleto = colectivo.PagarCon(tarjetaMedio);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(790, boleto.Monto);
            Assert.AreEqual(-790, tarjetaMedio.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoSinCosto()
        {
            TarjetaBoletoGratuito tarjetaGratuita = new TarjetaBoletoGratuito();
            Colectivo colectivo = new Colectivo("K");
            Boleto? boleto = colectivo.PagarCon(tarjetaGratuita);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto);
            Assert.AreEqual(0, tarjetaGratuita.Saldo);
        }

        [Test]
        public void TestBoletoGratuitoMultiplesViajes()
        {
            TarjetaBoletoGratuito tarjetaGratuita = new TarjetaBoletoGratuito();
            Colectivo colectivo = new Colectivo("143");
            for (int i = 0; i < 10; i++)
            {
                Boleto? boleto = colectivo.PagarCon(tarjetaGratuita);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(0, boleto.Monto);
            }
            Assert.AreEqual(0, tarjetaGratuita.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaSiemprePuedePagar()
        {
            TarjetaFranquiciaCompleta tarjetaJubilado = new TarjetaFranquiciaCompleta();
            Colectivo colectivo = new Colectivo("K");
            for (int i = 0; i < 50; i++)
            {
                Boleto? boleto = colectivo.PagarCon(tarjetaJubilado);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(0, boleto.Monto);
            }
            Assert.AreEqual(0, tarjetaJubilado.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaNoDescuentaSaldo()
        {
            TarjetaFranquiciaCompleta tarjetaJubilado = new TarjetaFranquiciaCompleta();
            tarjetaJubilado.Cargar(5000);
            Colectivo colectivo = new Colectivo("143");
            for (int i = 0; i < 5; i++)
            {
                colectivo.PagarCon(tarjetaJubilado);
            }
            Assert.AreEqual(5000, tarjetaJubilado.Saldo);
        }
    }
}