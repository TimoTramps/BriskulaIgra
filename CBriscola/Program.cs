/*
 *  This code is distribuited under GPL 3.0 or, at your opinion, any later version
 *  CBriscola 0.1
 *
 *  Created by numerunix on 22/05/22.
 *  Copyright 2022 Some rights reserved.
 *
 */

using System.Globalization;

namespace CBriscola {
	class Program
	{
		private static giocatore g;
		private static giocatore cpu;
		private static giocatore primo;
		private static giocatore secondo;
		private static mazzo m;
		public static System.Resources.ResourceManager mgr;
		private static UInt128 partite = 0;
		private static UInt16 puntiUtente = 0, puntiCpu = 0;
		private static int result;
		private static string s;
		public static void Main()
		{
			CreaResourceManager(CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
			elaboratoreCarteBriscola e = new elaboratoreCarteBriscola();
			m = new mazzo(e);
			carta.inizializza(40, cartaHelperBriscola.getIstanza(e));
			g = new giocatore(new giocatoreHelperUtente(), "", 3);
			cpu = new giocatore(new giocatoreHelperCpu(elaboratoreCarteBriscola.getCartaBriscola()), "", 3);
			primo = g;
			secondo = cpu;
			giocatore temp = g;
			carta c;
			carta c1;
			carta briscola = carta.getCarta(elaboratoreCarteBriscola.getCartaBriscola());
			string vers = "1.0";
			Console.WriteLine($"CBriscola {vers} {mgr.GetString("AdOperaDi")} Giulio Sorrentino. {mgr.GetString("Traduzione")} {mgr.GetString("AdOperaDi")} {mgr.GetString("Autore")}.");
			for (UInt16 i = 0; i < 3; i++)
			{
				g.addCarta(m);
				cpu.addCarta(m);
			}
			while (true)
			{
				if (m.getNumeroCarte()<UInt16.MaxValue) {
					System.Console.WriteLine($"{mgr.GetString("CartaBriscola")}: {briscola}");
					System.Console.WriteLine($"{mgr.GetString("CarteMazzo")}: {m.getNumeroCarte()} {mgr.GetString("carte")}.");
				}
				System.Console.WriteLine($"{mgr.GetString("PuntiCpu")}: {cpu.getPunteggio()}"); ;
				System.Console.WriteLine($"{mgr.GetString("PuntiUtente")}: {g.getPunteggio()}");
				gioca();
				c = primo.getCartaGiocata();
				c1 = secondo.getCartaGiocata();
				System.Console.WriteLine($" {c} {c1}");

				if ((c.CompareTo(c1) > 0 && c.stessoSeme(c1)) || (c1.stessoSeme(briscola) && !c.stessoSeme(briscola)))
				{
					temp = secondo;
					secondo = primo;
					primo = temp;
				}
				primo.aggiornaPunteggio(secondo);
				if (!aggiungiCarte())
				{
					if (partite == UInt128.MaxValue)
					{
						System.Console.WriteLine($"{mgr.GetString("GiocareTroppo")}");
						break;
					}
					partite++;
					if (partite % 2 == 1)
						System.Console.Write($"{mgr.GetString("SecondaPartita")}");
					else
						System.Console.Write($"{mgr.GetString("NuovaPartita")}");

					try
					{
                        s = System.Console.ReadLine();
                        result = int.Parse(s);
					} catch (Exception ex)
					{
						result = 0;
					}
					if (result!=1)
						break;
					else
					{
                        e = new elaboratoreCarteBriscola();
                        m = new mazzo(e);
                        carta.inizializza(40, cartaHelperBriscola.getIstanza(e));
                        g = new giocatore(new giocatoreHelperUtente(), "", 3);
                        cpu = new giocatore(new giocatoreHelperCpu(elaboratoreCarteBriscola.getCartaBriscola()), "", 3);
						if (partite % 2 == 1)
						{
							primo = cpu;
							secondo = g;
						} else
						{
							primo = g;
							secondo = cpu;
						}
                        briscola = carta.getCarta(elaboratoreCarteBriscola.getCartaBriscola());
                        for (UInt16 i = 0; i < 3; i++)
                        {
                            g.addCarta(m);
                            cpu.addCarta(m);
                        }
                    }
                }
            }
		}

		private static void gioca()
		{
			try
			{
				primo.gioca();
				if (primo == cpu)
					System.Console.WriteLine($"{mgr.GetString("Giocata")} {primo.getCartaGiocata()}");
				secondo.gioca(primo);
			}
			catch (System.ArgumentNullException e)
			{
			}

		}

		private static bool aggiungiCarte()
		{
			try
			{
				primo.addCarta(m);
				secondo.addCarta(m);
			}
			catch (IndexOutOfRangeException e)
			{
				puntiUtente += g.getPunteggio();
				puntiCpu += cpu.getPunteggio();
				System.Console.WriteLine($"{mgr.GetString("PartitaFinita")}.");
				System.Console.WriteLine($"{mgr.GetString("PuntiUtente")}: {puntiUtente}");
				System.Console.WriteLine($"{mgr.GetString("PuntiCpu")}: {puntiCpu}");
				if (puntiUtente == puntiCpu)
					Console.WriteLine($"{mgr.GetString("PartitaPatta")}.");
				else
					if (puntiUtente > puntiCpu)
						Console.WriteLine($"{mgr.GetString("HaiVintoPer")} {puntiUtente - puntiCpu} {mgr.GetString("punti")}.");
					else
						Console.WriteLine($"{mgr.GetString("HaiPersoPer")} {puntiCpu - puntiUtente} {mgr.GetString("punti")}.");

				return false;
			}
			return true;
		}
		private static void CreaResourceManager(string arg) {
			System.Resources.ResourceManager m;
			m = new System.Resources.ResourceManager($"CBriscola.Strings.{arg}.Resources", System.Reflection.Assembly.GetExecutingAssembly());
			try
			{
				m.GetString("di");
			}
			catch (System.Resources.MissingManifestResourceException e)
			{
				m = new System.Resources.ResourceManager($"CBriscola.Strings.it.Resources", System.Reflection.Assembly.GetExecutingAssembly());
			}
			mgr = m;
		}
	}
}
