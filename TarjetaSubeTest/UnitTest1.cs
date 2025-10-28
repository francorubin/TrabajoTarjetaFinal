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
        // ============== TESTS PARA MEDIO BOLETO ==============

        [Test]
        public void TestMedioBoletoMontoPagadoEsMitad()
        {
            // Test que valida que el monto pagado es siempre la mitad
            TarjetaMedioBoleto tarjetaMedio = new TarjetaMedioBoleto();
            tarjetaMedio.Cargar(5000);

            Colectivo colectivo = new Colectivo("K");
            Boleto? boleto = colectivo.PagarCon(tarjetaMedio);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(790, boleto.Monto); // $1580 / 2 = $790
            Assert.AreEqual(4210, tarjetaMedio.Saldo); // $5000 - $790 = $4210
        }

        [Test]
        public void TestMedioBoletoMultiplesViajes()
        {
            TarjetaMedioBoleto tarjetaMedio = new TarjetaMedioBoleto();
            tarjetaMedio.Cargar(3000);

            Colectivo colectivo = new Colectivo("143");

            // Primer viaje: $790
            Boleto? boleto1 = colectivo.PagarCon(tarjetaMedio);
            Assert.IsNotNull(boleto1);
            Assert.AreEqual(790, boleto1.Monto);

            // Segundo viaje: $790
            Boleto? boleto2 = colectivo.PagarCon(tarjetaMedio);
            Assert.IsNotNull(boleto2);
            Assert.AreEqual(790, boleto2.Monto);

            // Tercer viaje: $790
            Boleto? boleto3 = colectivo.PagarCon(tarjetaMedio);
            Assert.IsNotNull(boleto3);
            Assert.AreEqual(790, boleto3.Monto);

            // Saldo final: $3000 - (3 * $790) = $630
            Assert.AreEqual(630, tarjetaMedio.Saldo);
        }

        [Test]
        public void TestMedioBoletoConSaldoNegativo()
        {
            TarjetaMedioBoleto tarjetaMedio = new TarjetaMedioBoleto();

            Colectivo colectivo = new Colectivo("K");

            // Pagar sin saldo (debería quedar en -790)
            Boleto? boleto = colectivo.PagarCon(tarjetaMedio);
            Assert.IsNotNull(boleto);
            Assert.AreEqual(790, boleto.Monto);
            Assert.AreEqual(-790, tarjetaMedio.Saldo);
        }

        // ============== TESTS PARA BOLETO GRATUITO ==============

        [Test]
        public void TestBoletoGratuitoSinCosto()
        {
            TarjetaBoletoGratuito tarjetaGratuita = new TarjetaBoletoGratuito();

            Colectivo colectivo = new Colectivo("K");
            Boleto? boleto = colectivo.PagarCon(tarjetaGratuita);

            Assert.IsNotNull(boleto);
            Assert.AreEqual(0, boleto.Monto); // El monto debe ser $0
            Assert.AreEqual(0, tarjetaGratuita.Saldo); // El saldo no cambia
        }

        [Test]
        public void TestBoletoGratuitoMultiplesViajes()
        {
            TarjetaBoletoGratuito tarjetaGratuita = new TarjetaBoletoGratuito();

            Colectivo colectivo = new Colectivo("143");

            // Realizar 10 viajes
            for (int i = 0; i < 10; i++)
            {
                Boleto? boleto = colectivo.PagarCon(tarjetaGratuita);
                Assert.IsNotNull(boleto);
                Assert.AreEqual(0, boleto.Monto);
            }

            // El saldo siempre debe ser 0
            Assert.AreEqual(0, tarjetaGratuita.Saldo);
        }

        // ============== TESTS PARA FRANQUICIA COMPLETA ==============

        [Test]
        public void TestFranquiciaCompletaSiemprePuedePagar()
        {
            // Test que valida que franquicia completa siempre puede pagar
            TarjetaFranquiciaCompleta tarjetaJubilado = new TarjetaFranquiciaCompleta();

            Colectivo colectivo = new Colectivo("K");

            // Realizar 50 viajes sin cargar saldo
            for (int i = 0; i < 50; i++)
            {
                Boleto? boleto = colectivo.PagarCon(tarjetaJubilado);
                Assert.IsNotNull(boleto, $"El viaje {i + 1} debería poder realizarse");
                Assert.AreEqual(0, boleto.Monto);
            }

            // El saldo siempre debe ser 0
            Assert.AreEqual(0, tarjetaJubilado.Saldo);
        }

        [Test]
        public void TestFranquiciaCompletaNoDescuentaSaldo()
        {
            TarjetaFranquiciaCompleta tarjetaJubilado = new TarjetaFranquiciaCompleta();

            // Cargar saldo (aunque no sea necesario)
            tarjetaJubilado.Cargar(5000);

            Colectivo colectivo = new Colectivo("143");

            // Realizar 5 viajes
            for (int i = 0; i < 5; i++)
            {
                colectivo.PagarCon(tarjetaJubilado);
            }

            // El saldo debe seguir siendo $5000
            Assert.AreEqual(5000, tarjetaJubilado.Saldo);
        }
    }
    


    }