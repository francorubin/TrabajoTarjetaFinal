using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaSube
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Tarjeta SUBE - Rosario ===\n");
            Console.WriteLine("Seleccione el modo:");
            Console.WriteLine("1. Modo Interactivo (puedes cargar y pagar manualmente)");
            Console.WriteLine("2. Modo Demo Automático (demostración predefinida)");
            Console.Write("\nIngrese su opción (1 o 2): ");

            string? modoSeleccionado = Console.ReadLine();

            if (modoSeleccionado == "1")
            {
                ModoInteractivo();
            }
            else if (modoSeleccionado == "2")
            {
                ModoDemoAutomatico();
            }
            else
            {
                Console.WriteLine("\nOpción inválida. Ejecutando modo interactivo por defecto...\n");
                System.Threading.Thread.Sleep(1500);
                ModoInteractivo();
            }
        }

        static void ModoInteractivo()
        {
            Tarjeta miTarjeta = new Tarjeta();
            bool continuar = true;

            Console.Clear();
            Console.WriteLine("=== MODO INTERACTIVO ===\n");

            while (continuar)
            {
                Console.WriteLine("\n╔════════════════════════════════════╗");
                Console.WriteLine("║           MENÚ PRINCIPAL           ║");
                Console.WriteLine("╚════════════════════════════════════╝");
                Console.WriteLine($"Saldo actual: ${miTarjeta.Saldo}");
                Console.WriteLine("\n1. Cargar saldo");
                Console.WriteLine("2. Pagar viaje en colectivo");
                Console.WriteLine("3. Ver información de saldo");
                Console.WriteLine("4. Salir");
                Console.Write("\nSeleccione una opción: ");

                string? opcion = Console.ReadLine();
                Console.WriteLine();

                switch (opcion)
                {
                    case "1":
                        CargarSaldo(miTarjeta);
                        break;

                    case "2":
                        PagarViaje(miTarjeta);
                        break;

                    case "3":
                        MostrarInformacionSaldo(miTarjeta);
                        break;

                    case "4":
                        Console.WriteLine("╔════════════════════════════════════╗");
                        Console.WriteLine("║   ¡Gracias por usar SUBE Rosario!  ║");
                        Console.WriteLine("╚════════════════════════════════════╝");
                        continuar = false;
                        break;

                    default:
                        Console.WriteLine("Opción inválida. Por favor seleccione 1, 2, 3 o 4.");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void CargarSaldo(Tarjeta tarjeta)
        {
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║            CARGAR SALDO            ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.WriteLine("\nMontos permitidos:");
            Console.WriteLine("   • $2000   • $3000   • $4000   • $5000");
            Console.WriteLine("   • $8000   • $10000  • $15000  • $20000");
            Console.WriteLine("   • $25000  • $30000");
            Console.WriteLine($"\nLímite máximo de saldo: $40000");
            Console.WriteLine($"Tu saldo actual: ${tarjeta.Saldo}");
            Console.Write("\nIngrese el monto a cargar: $");

            if (decimal.TryParse(Console.ReadLine(), out decimal monto))
            {
                if (tarjeta.Cargar(monto))
                {
                    Console.WriteLine($"\n¡Carga exitosa!");
                    Console.WriteLine($"Monto cargado: ${monto}");
                    Console.WriteLine($"Nuevo saldo: ${tarjeta.Saldo}");
                }
                else
                {
                    Console.WriteLine("\nCarga rechazada");
                    if (tarjeta.Saldo + monto > 40000)
                    {
                        Console.WriteLine($"Motivo: Superarías el límite de $40000");
                        Console.WriteLine($"Máximo que puedes cargar: ${40000 - tarjeta.Saldo}");
                    }
                    else
                    {
                        Console.WriteLine($"Motivo: ${monto} no es un monto permitido");
                    }
                }
            }
            else
            {
                Console.WriteLine("Monto inválido. Debe ser un número.");
            }
        }

        static void PagarViaje(Tarjeta tarjeta)
        {
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║      PAGAR VIAJE EN COLECTIVO      ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.WriteLine($"\nSaldo disponible: ${tarjeta.Saldo}");
            Console.WriteLine($"Tarifa: $1580");

            if (tarjeta.Saldo < 1580 && tarjeta.Saldo >= -1200)
            {
                Console.WriteLine("\nUsando VIAJE PLUS (saldo negativo permitido hasta -$1200)");
            }
            else if (tarjeta.Saldo < -1200)
            {
                Console.WriteLine("\nSALDO INSUFICIENTE");
                Console.WriteLine($"Has alcanzado el límite de saldo negativo");
                Console.WriteLine("Por favor, carga saldo primero.");
                return;
            }

            Console.Write("\nIngrese la línea de colectivo (ej: K, 143, 102): ");
            string? linea = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(linea))
            {
                Console.WriteLine("Debe ingresar una línea válida.");
                return;
            }

            Colectivo colectivo = new Colectivo(linea);
            Boleto? boleto = colectivo.PagarCon(tarjeta);

            if (boleto != null)
            {
                Console.WriteLine("\n╔════════════════════════════════════╗");
                Console.WriteLine("║            VIAJE PAGADO            ║");
                Console.WriteLine("╚════════════════════════════════════╝");
                Console.WriteLine($"Línea: {boleto.LineaColectivo}");
                Console.WriteLine($"Monto pagado: ${boleto.Monto}");
                Console.WriteLine($"Saldo restante: ${boleto.SaldoRestante}");
                Console.WriteLine($"Fecha y hora: {boleto.Fecha:dd/MM/yyyy HH:mm:ss}");

                if (boleto.SaldoRestante < 0)
                {
                    Console.WriteLine($"\n⚠ VIAJE PLUS utilizado. Saldo negativo: ${boleto.SaldoRestante}");
                }
            }
            else
            {
                Console.WriteLine("\n❌ Viaje rechazado: saldo insuficiente");
                Console.WriteLine("Has alcanzado el límite de saldo negativo (-$1200)");
            }
        }

        static void MostrarInformacionSaldo(Tarjeta tarjeta)
        {
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║        INFORMACIÓN DE SALDO        ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.WriteLine($"\nSaldo actual: ${tarjeta.Saldo}");

            if (tarjeta.Saldo >= 0)
            {
                Console.WriteLine($"Viajes disponibles: {(int)(tarjeta.Saldo / 1580)}");
            }
            else
            {
                Console.WriteLine($"Saldo negativo (VIAJE PLUS usado)");
                Console.WriteLine($"Crédito utilizado: ${Math.Abs(tarjeta.Saldo)}");
                Console.WriteLine($"Crédito disponible: ${1200 + tarjeta.Saldo}");
            }

            Console.WriteLine($"Límite máximo: $40000");
            Console.WriteLine($"Disponible para cargar: ${40000 - Math.Max(0, tarjeta.Saldo)}");

            if (tarjeta.Saldo < -1200)
            {
                Console.WriteLine($"\n⚠ LÍMITE ALCANZADO: No puedes viajar hasta cargar saldo");
            }
            else if (tarjeta.Saldo < 0)
            {
                Console.WriteLine($"\n⚠ Saldo negativo. Al cargar, se descontará primero esta deuda.");
            }
            else if (tarjeta.Saldo < 1580)
            {
                Console.WriteLine($"\nℹ Puedes usar VIAJE PLUS (1 viaje con saldo negativo)");
            }
            else if (tarjeta.Saldo < 3160)
            {
                Console.WriteLine($"\nℹ Saldo bajo: Solo te alcanza para {(int)(tarjeta.Saldo / 1580)} viaje(s)");
            }
        }

        static void ModoDemoAutomatico()
        {
            Console.Clear();
            Console.WriteLine("=== MODO DEMO AUTOMÁTICO ===\n");

            Tarjeta miTarjeta = new Tarjeta();
            Console.WriteLine($"Saldo inicial: ${miTarjeta.Saldo}");

            Console.WriteLine("\n--- Cargando saldo ---");
            if (miTarjeta.Cargar(5000))
            {
                Console.WriteLine($"✓ Carga exitosa de $5000");
                Console.WriteLine($"Saldo actual: ${miTarjeta.Saldo}");
            }

            Console.WriteLine("\n--- Intentando carga inválida ---");
            if (!miTarjeta.Cargar(1500))
            {
                Console.WriteLine("✗ Carga de $1500 rechazada (monto no permitido)");
            }

            Colectivo colectivo = new Colectivo("K");
            Console.WriteLine($"\n--- Viajando en colectivo línea {colectivo.Linea} ---");

            for (int i = 1; i <= 3; i++)
            {
                Boleto? boleto = colectivo.PagarCon(miTarjeta);
                if (boleto != null)
                {
                    Console.WriteLine($"\n✓ Viaje {i}:");
                    Console.WriteLine($"  Línea: {boleto.LineaColectivo}");
                    Console.WriteLine($"  Monto pagado: ${boleto.Monto}");
                    Console.WriteLine($"  Saldo restante: ${boleto.SaldoRestante}");
                    Console.WriteLine($"  Fecha: {boleto.Fecha:dd/MM/yyyy HH:mm:ss}");
                }
            }

            Console.WriteLine($"\n--- Resumen ---");
            Console.WriteLine($"Saldo final en tarjeta: ${miTarjeta.Saldo}");
            Console.WriteLine($"Total gastado: ${5000 - miTarjeta.Saldo}");

            Console.WriteLine("\n--- Intentando viajar sin saldo suficiente ---");
            Tarjeta tarjetaVacia = new Tarjeta();
            Boleto? boletoConViajePlus = colectivo.PagarCon(tarjetaVacia);
            if (boletoConViajePlus != null)
            {
                Console.WriteLine($"✓ VIAJE PLUS usado. Saldo: ${tarjetaVacia.Saldo}");
            }

            Console.WriteLine("\n--- Probando diferentes tipos de tarjetas ---");

            // Medio Boleto
            TarjetaMedioBoleto tarjetaMedio = new TarjetaMedioBoleto();
            tarjetaMedio.Cargar(2000);
            Boleto? boletoMedio = colectivo.PagarCon(tarjetaMedio);
            if (boletoMedio != null)
            {
                Console.WriteLine($"\n✓ MEDIO BOLETO:");
                Console.WriteLine($"  Monto pagado: ${boletoMedio.Monto} (50% de descuento)");
                Console.WriteLine($"  Saldo restante: ${tarjetaMedio.Saldo}");
            }

            // Boleto Gratuito
            TarjetaBoletoGratuito tarjetaGratuita = new TarjetaBoletoGratuito();
            Boleto? boletoGratuito = colectivo.PagarCon(tarjetaGratuita);
            if (boletoGratuito != null)
            {
                Console.WriteLine($"\n✓ BOLETO GRATUITO:");
                Console.WriteLine($"  Monto pagado: ${boletoGratuito.Monto}");
                Console.WriteLine($"  Saldo restante: ${tarjetaGratuita.Saldo}");
            }

            // Franquicia Completa
            TarjetaFranquiciaCompleta tarjetaJubilado = new TarjetaFranquiciaCompleta();
            Boleto? boletoJubilado = colectivo.PagarCon(tarjetaJubilado);
            if (boletoJubilado != null)
            {
                Console.WriteLine($"\n✓ FRANQUICIA COMPLETA (Jubilado):");
                Console.WriteLine($"  Monto pagado: ${boletoJubilado.Monto}");
                Console.WriteLine($"  Saldo restante: ${tarjetaJubilado.Saldo}");
            }

            Console.WriteLine("\n=== Presiona cualquier tecla para salir ===");
            Console.ReadKey();
        }
    }
}