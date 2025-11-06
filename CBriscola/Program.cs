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
		private static giocatore first;
		private static giocatore second;
		private static mazzo m;
		public static System.Resources.ResourceManager mgr;
		private static UInt128 rounds = 0;
		private static UInt16 points = 0, pointsCpu = 0;
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
			first = g;
			second = cpu;
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
				System.Console.WriteLine($"{mgr.GetString("pointsCpu")}: {cpu.getPunteggio()}"); ;
				System.Console.WriteLine($"{mgr.GetString("points")}: {g.getPunteggio()}");
				gioca();
				c = first.getCartaGiocata();
				c1 = second.getCartaGiocata();
				System.Console.WriteLine($" {c} {c1}");

				if ((c.CompareTo(c1) > 0 && c.stessoSeme(c1)) || (c1.stessoSeme(briscola) && !c.stessoSeme(briscola)))
				{
					temp = second;
					second = first;
					first = temp;
				}
				first.aggiornaPunteggio(second);
				if (!aggiungiCarte())
				{
					if (rounds == UInt128.MaxValue)
					{
						System.Console.WriteLine($"{mgr.GetString("GiocareTroppo")}");
						break;
					}
					rounds++;
					if (rounds % 2 == 1)
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
						if (rounds % 2 == 1)
						{
							first = cpu;
							second = g;
						} else
						{
							first = g;
							second = cpu;
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
				first.gioca();
				if (first == cpu)
					System.Console.WriteLine($"{mgr.GetString("Giocata")} {first.getCartaGiocata()}");
				second.gioca(first);
			}
			catch (System.ArgumentNullException e)
			{
			}

		}

		private static bool aggiungiCarte()
		{
			try
			{
				first.addCarta(m);
				second.addCarta(m);
			}
			catch (IndexOutOfRangeException e)
			{
				points += g.getPunteggio();
				pointsCpu += cpu.getPunteggio();
				System.Console.WriteLine($"{mgr.GetString("PartitaFinita")}.");
				System.Console.WriteLine($"{mgr.GetString("points")}: {points}");
				System.Console.WriteLine($"{mgr.GetString("pointsCpu")}: {pointsCpu}");
				if (points == pointsCpu)
					Console.WriteLine($"{mgr.GetString("PartitaPatta")}.");
				else
					if (points > pointsCpu)
						Console.WriteLine($"{mgr.GetString("HaiVintoPer")} {points - pointsCpu} {mgr.GetString("punti")}.");
					else
						Console.WriteLine($"{mgr.GetString("HaiPersoPer")} {pointsCpu - points} {mgr.GetString("punti")}.");

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

