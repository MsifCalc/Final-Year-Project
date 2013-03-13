using System;
using System.Linq;
using System.Windows;
using MeltCalc.Chemistry;
using MeltCalc.Model;
using MeltCalc.Helpers;

namespace MeltCalc.ViewModel
{
	public class AdaptationModel : BasePresenter
	{
		private static int NumberOfAdaptedMelt, adaptROUND, BaseLenth, GoodMeltsQuant;
		private static double[] minL, maxL, minalfaFe, maxalfaFe, minTeplFutLoss, maxTeplFutLoss = new double[7];
		private static double[] stepL, stepalfaFe, stepTeplFutLoss, GshlSAVE, LSAVE, alfaFeSAVE, TeplFutLossSAVE = new double[6];
		private static double CheckNoNull, adaptMistakeTOTAL, adaptCOMPAIR, TempLeft, TempRight;
		private static double SummGshl, ExpectGshl, SummL, ExpectL, SummAlfaFe, ExpectAlfaFe, SummTeplFutLoss, ExpectTeplFutLoss;

		private const double PConst = 62.0 / 142.0;

		private readonly ParamsMdb _paramsMdb = new ParamsMdb();
		private readonly MeltMdb _meltMdb = new MeltMdb();
		// Расчет с фиксированной массой жидкого.
		private bool _isFixedMass;

		public void Run(int sypuchIndex, bool fixedMass)
		{
			_isFixedMass = fixedMass;
			GoodMeltsQuant = 0;

			try
			{
				LoadFromCountData(Params.SelectedPlant);
				LoadFromLoose(sypuchIndex);
				LoadMixAndOstShl();
				DetectBaseLenth();
				InternalRun();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void InternalRun()
		{
			for (NumberOfAdaptedMelt = 0; NumberOfAdaptedMelt <= BaseLenth; NumberOfAdaptedMelt++)
			{
				LoadMeltData(NumberOfAdaptedMelt);
				Params.Tog = Tube.Сталь.T;
				Tube.Сталь.Si = 0.003;
				Tube.Шлак.FeO = 0.701145 * Tube.Шлак.TOTALFeO - 0.586142;
				Tube.Шлак.Fe2O3 = Tube.Шлак.TOTALFeO - Tube.Шлак.FeO;
				Estimation.CALCULATE_TEPLCONSTANTS();

				// Значения диапазонов сканирования.
				Estimation.minGshl[0] = 0.07 * Tube.Сталь.GYield;
				Estimation.maxGshl[0] = 0.2 * Tube.Сталь.GYield;
				minL[0] = 0.1;
				maxL[0] = 0.3;
				minalfaFe[0] = 0.005;
				maxalfaFe[0] = 0.05;
				minTeplFutLoss[0] = -10000000;
				maxTeplFutLoss[0] = 60000000;
				Estimation.minGstYield[0] = 0.8 * Tube.Сталь.GYield;
				Estimation.maxGstYield[0] = 1.2 * Tube.Сталь.GYield;
				Estimation.minALFAizv[0] = 0.2;
				Estimation.maxALFAizv[0] = 1.0;

				// Расчет шагов сканирования для всех уточнений.
				Estimation.stepGshl[0] = (Estimation.maxGshl[0] - Estimation.minGshl[0]) / 4.0;
				stepL[0] = (maxL[0] - minL[0]) / 4.0;
				stepalfaFe[0] = (maxalfaFe[0] - minalfaFe[0]) / 4.0;
				stepTeplFutLoss[0] = (maxTeplFutLoss[0] - minTeplFutLoss[0]) / 4.0;
				Estimation.stepGstYield[0] = (Estimation.maxGstYield[0] - Estimation.minGstYield[0]) / 4.0;
				Estimation.stepALFAizv[0] = (Estimation.maxALFAizv[0] - Estimation.minALFAizv[0]) / 4.0;

				// Расчет для 1 - 5 кругов вычислений.
				for (var i = 1; i < 6; i++)
				{
					Estimation.stepGshl[i] = Estimation.stepGshl[i - 1] / 2.0;
					stepL[i] = stepL[i - 1] / 2.0;
					stepalfaFe[i] = stepalfaFe[i - 1] / 2.0;
					stepTeplFutLoss[i] = stepTeplFutLoss[i - 1] / 6.0;
					Estimation.stepGstYield[i] = Estimation.stepGstYield[i - 1] / 2.0;
					Estimation.stepALFAizv[i] = Estimation.stepALFAizv[i - 1] / 2.0;
				}

				adaptCOMPAIR = 100;

				for (adaptROUND = 0; adaptROUND < 6; adaptROUND++)
				{
					if (_isFixedMass)
					{
						Tube.Шлак.G = Estimation.minGshl[adaptROUND];
						Params.L = minL[adaptROUND];
						Params.alfaFe = minalfaFe[adaptROUND];
						Params.TeplFutLoss = minTeplFutLoss[adaptROUND];

						do
						{
							do
							{
								do
								{
									do
									{
										Estimation.Balances_Calc();
										adaptMistakeTOTAL = Math.Abs((Estimation.LeftSHL - Estimation.RightSHL) / Estimation.RightSHL) +
															Math.Abs((Estimation.LeftTEPL - Estimation.RightTEPL) / Estimation.RightTEPL) +
															Math.Abs((Estimation.LeftO2 - Estimation.RightO2) / Estimation.RightO2) +
															Math.Abs((Estimation.LeftMAT - Estimation.RightMAT) / Estimation.RightMAT);

										if (adaptMistakeTOTAL <= adaptCOMPAIR)
										{
											adaptCOMPAIR = adaptMistakeTOTAL;
											GshlSAVE[adaptROUND] = Tube.Шлак.G;
											LSAVE[adaptROUND] = Params.L;
											alfaFeSAVE[adaptROUND] = Params.alfaFe;
											TeplFutLossSAVE[adaptROUND] = Params.TeplFutLoss;
										}

										Params.TeplFutLoss += stepTeplFutLoss[adaptROUND];

									} while (Params.TeplFutLoss <= maxTeplFutLoss[adaptROUND]);

									Params.TeplFutLoss = minTeplFutLoss[adaptROUND];
									Params.alfaFe += stepalfaFe[adaptROUND];

								} while (Params.alfaFe <= maxalfaFe[adaptROUND]);

								Params.alfaFe = minalfaFe[adaptROUND];
								Params.L += stepL[adaptROUND];

							} while (Params.L <= maxL[adaptROUND]);

							Params.L = minL[adaptROUND];
							Tube.Шлак.G += Estimation.stepGshl[adaptROUND];

						} while (Tube.Шлак.G <= Estimation.maxGshl[adaptROUND]);

						Tube.Шлак.G = Estimation.minGshl[adaptROUND];

						Estimation.minGshl[adaptROUND + 1] = GshlSAVE[adaptROUND] - Estimation.stepGshl[adaptROUND];
						minL[adaptROUND + 1] = LSAVE[adaptROUND] - stepL[adaptROUND];
						minalfaFe[adaptROUND + 1] = alfaFeSAVE[adaptROUND] - stepalfaFe[adaptROUND];
						minTeplFutLoss[adaptROUND + 1] = TeplFutLossSAVE[adaptROUND] - stepTeplFutLoss[adaptROUND];
						Estimation.maxGshl[adaptROUND + 1] = GshlSAVE[adaptROUND] + Estimation.stepGshl[adaptROUND];
						maxL[adaptROUND + 1] = LSAVE[adaptROUND] + stepL[adaptROUND];
						maxalfaFe[adaptROUND + 1] = alfaFeSAVE[adaptROUND] + stepalfaFe[adaptROUND];
						maxTeplFutLoss[adaptROUND + 1] = TeplFutLossSAVE[adaptROUND] + stepTeplFutLoss[adaptROUND];
					}
					else
					{
						// Расчет при неизвестной(плавающей массе продукта)
						Tube.Сталь.G = Estimation.minGstYield[adaptROUND];
						Tube.Шлак.G = Estimation.minGshl[adaptROUND];
						Tube.Известь.ALFA = Estimation.minALFAizv[adaptROUND];
						Params.L = minL[adaptROUND];
						Params.TeplFutLoss = minTeplFutLoss[adaptROUND];

						Params.alfaFe = 0.013;

						do
						{
							do
							{
								do
								{
									do
									{
										do
										{
											Estimation.Balances_Calc();
											adaptMistakeTOTAL =
												Math.Abs((Estimation.LeftCaOiMgO - Estimation.RightCaOiMgO) / Estimation.RightCaOiMgO) +
												Math.Abs((Estimation.LeftSHL - Estimation.RightSHL) / Estimation.RightSHL) +
												Math.Abs((Estimation.LeftTEPL - Estimation.RightTEPL) / Estimation.RightTEPL) +
												Math.Abs((Estimation.LeftO2 - Estimation.RightO2) / Estimation.RightO2) +
												Math.Abs((Estimation.LeftMAT - Estimation.RightMAT) / Estimation.RightMAT) +
												Math.Abs((Estimation.LeftSi - Estimation.RightSi) / Estimation.RightSi) +
												Math.Abs((Estimation.LeftMn - Estimation.RightMn) / Estimation.RightMn);

											if (adaptMistakeTOTAL <= adaptCOMPAIR)
											{
												adaptCOMPAIR = adaptMistakeTOTAL;
												Estimation.GstYieldSAVE[adaptROUND] = Tube.Сталь.GYield;
												GshlSAVE[adaptROUND] = Tube.Шлак.G;
												LSAVE[adaptROUND] = Params.L;
												Estimation.ALFAizvSAVE[adaptROUND] = Tube.Известь.ALFA;
												TeplFutLossSAVE[adaptROUND] = Params.TeplFutLoss;
											}

											Params.TeplFutLoss += stepTeplFutLoss[adaptROUND];

										} while (Params.TeplFutLoss <= maxTeplFutLoss[adaptROUND]);

										Params.TeplFutLoss = minTeplFutLoss[adaptROUND];
										Tube.Известь.ALFA += Estimation.stepALFAizv[adaptROUND];

									} while (Tube.Известь.ALFA <= Estimation.maxALFAizv[adaptROUND]);

									Tube.Известь.ALFA = Estimation.minALFAizv[adaptROUND];
									Params.L += stepL[adaptROUND];

								} while (Params.L <= maxL[adaptROUND]);

								Params.L = minL[adaptROUND];
								Tube.Шлак.G += Estimation.stepGshl[adaptROUND];

							} while (Tube.Шлак.G <= Estimation.maxGshl[adaptROUND]);

							Tube.Шлак.G = Estimation.minGshl[adaptROUND];
							Tube.Сталь.GYield += Estimation.stepGstYield[adaptROUND];

						} while (Tube.Сталь.GYield <= Estimation.maxGstYield[adaptROUND]);

						Tube.Сталь.GYield = Estimation.minGstYield[adaptROUND];

						Estimation.minGstYield[adaptROUND + 1] = Estimation.GstYieldSAVE[adaptROUND] - Estimation.stepGstYield[adaptROUND];
						Estimation.minGshl[adaptROUND + 1] = GshlSAVE[adaptROUND] - Estimation.stepGshl[adaptROUND];
						minL[adaptROUND + 1] = LSAVE[adaptROUND] - stepL[adaptROUND];
						Estimation.minALFAizv[adaptROUND + 1] = Estimation.ALFAizvSAVE[adaptROUND] - Estimation.stepALFAizv[adaptROUND];
						minTeplFutLoss[adaptROUND + 1] = TeplFutLossSAVE[adaptROUND] - stepTeplFutLoss[adaptROUND];
						
						// Верхняя граница
						Estimation.maxGstYield[adaptROUND + 1] = Estimation.GstYieldSAVE[adaptROUND] + Estimation.stepGstYield[adaptROUND];
						Estimation.maxGshl[adaptROUND + 1] = GshlSAVE[adaptROUND] + Estimation.stepGshl[adaptROUND];
						maxL[adaptROUND + 1] = LSAVE[adaptROUND] + stepL[adaptROUND];
						Estimation.maxALFAizv[adaptROUND + 1] = Estimation.ALFAizvSAVE[adaptROUND] + Estimation.stepALFAizv[adaptROUND];
						maxTeplFutLoss[adaptROUND + 1] = TeplFutLossSAVE[adaptROUND] + stepTeplFutLoss[adaptROUND];

						// Проверка нижней границы на неотрицательность
						if (Estimation.minGstYield[adaptROUND + 1] < Estimation.minGstYield[0])
						{
							Estimation.minGstYield[adaptROUND + 1] = Estimation.minGstYield[0];
							Estimation.maxGstYield[adaptROUND + 1] = Estimation.minGstYield[adaptROUND + 1] +
																	 2 * Estimation.stepGstYield[adaptROUND];
						}

						if (Estimation.minGshl[adaptROUND + 1] < Estimation.minGshl[0])
						{
							Estimation.minGshl[adaptROUND + 1] = Estimation.minGshl[0];
							Estimation.maxGshl[adaptROUND + 1] = Estimation.minGshl[adaptROUND + 1] + 2 * Estimation.stepGshl[adaptROUND];
						}

						if (minL[adaptROUND + 1] < minL[0])
						{
							minL[adaptROUND + 1] = minL[0];
							maxL[adaptROUND + 1] = minL[adaptROUND + 1] + 2 * stepL[adaptROUND];
						}

						if (Estimation.minALFAizv[adaptROUND + 1] < Estimation.minALFAizv[0])
						{
							Estimation.minALFAizv[adaptROUND + 1] = Estimation.minALFAizv[0];
							Estimation.maxALFAizv[adaptROUND + 1] = Estimation.minALFAizv[adaptROUND + 1] +
																	2 * Estimation.stepALFAizv[adaptROUND];
						}

						if (minTeplFutLoss[adaptROUND + 1] < minTeplFutLoss[0])
						{
							minTeplFutLoss[adaptROUND + 1] = minTeplFutLoss[0];
							maxTeplFutLoss[adaptROUND + 1] = minTeplFutLoss[adaptROUND + 1] + 2 * stepTeplFutLoss[adaptROUND];
						}

						Tube.Шлак.P2O5 = (Tube.Чугун.G * Tube.Чугун.P + Tube.Лом.G * Tube.Лом.P +
										  Tube.Известь.ALFA * Tube.Известь.G * Tube.Известь.P2O5 * 62.0 / 142.0 +
										  Tube.Известняк.ALFA * Tube.Известняк.G * Tube.Известняк.P2O5 * 62.0 / 142.0 +
										  Tube.Окалина.ALFA * Tube.Окалина.G * Tube.Окалина.P + Tube.Руда.ALFA * Tube.Руда.G * Tube.Руда.P +
										  Tube.Футеровка.G * Tube.Футеровка.P2O5 * 62.0 / 142.0 +
										  Tube.ОставленныйШлак.G * Tube.ОставленныйШлак.P2O5 * 62.0 / 142.0 +
										  Tube.МиксерныйШлак.G * Tube.МиксерныйШлак.P2O5 * 62.0 / 142.0 +
										  Tube.Ферросплав.ALFA * Tube.Ферросплав.G * Tube.Ферросплав.P -
										  (Estimation.GstYieldSAVE[adaptROUND] / (1 - Tube.Ферросплав.ALFA - Params.StAndShlLoss)) *
										  Tube.Сталь.P) /
										 (GshlSAVE[adaptROUND] * 62.0 / 142.0);
					}
				}

				// Выполняются только при первом варианте адаптации
				if (_isFixedMass)
				{
					// Расчет основности шлака
					TempLeft =
						Tube.Известь.ALFA * Tube.Известь.G * (Tube.Известь.CaO + Tube.Известь.MgO) +
						Tube.Известняк.ALFA * Tube.Известняк.G * 56 / 100 * Tube.Известняк.CaCO3 +
						Tube.Доломит.ALFA * Tube.Доломит.G * (Tube.Доломит.CaO + Tube.Доломит.MgO) +
						Tube.Шпат.ALFA * Tube.Шпат.G * Tube.Шпат.CaO + Tube.Окалина.ALFA * Tube.Окалина.G * Tube.Окалина.MgO +
						Tube.ВлажныйДоломит.ALFA * Tube.ВлажныйДоломит.G * (Tube.ВлажныйДоломит.CaO + Tube.ВлажныйДоломит.MgO) +
						Tube.Имф.ALFA * Tube.Имф.G * (Tube.Имф.CaO + Tube.Имф.MgO) +
						Tube.Футеровка.G * (Tube.Футеровка.CaO + Tube.Футеровка.MgO) +
						Tube.Агломерат.ALFA * Tube.Агломерат.G * Tube.Агломерат.CaO + Tube.Руда.ALFA * Tube.Руда.G * Tube.Руда.CaO +
						Tube.ОставленныйШлак.G * (Tube.ОставленныйШлак.CaO + Tube.ОставленныйШлак.MgO) +
						Tube.МиксерныйШлак.G * (Tube.МиксерныйШлак.CaO + Tube.МиксерныйШлак.MgO);

					TempRight =
						60 / 28 *
						(Tube.Чугун.G * Tube.Чугун.Si + Tube.Лом.G * Tube.Лом.Si +
						 Tube.Ферросплав.ALFA * Tube.Ферросплав.G * Tube.Ферросплав.Si -
						 (Tube.Сталь.GYield / (1 - alfaFeSAVE[adaptROUND - 1] - Params.StAndShlLoss)) * Tube.Сталь.Si) + 
						 Tube.ОставленныйШлак.G * Tube.ОставленныйШлак.SiO2 + Tube.МиксерныйШлак.G * Tube.МиксерныйШлак.SiO2 +
						(Tube.Доломит.ALFA * Tube.Доломит.G * Tube.Доломит.SiO2 +
						 Tube.ВлажныйДоломит.ALFA * Tube.ВлажныйДоломит.G * Tube.ВлажныйДоломит.SiO2 +
						 Tube.Имф.ALFA * Tube.Имф.G * Tube.Имф.SiO2) +
						(Tube.Шпат.ALFA * Tube.Шпат.G * Tube.Шпат.SiO2 + Tube.Известь.ALFA * Tube.Известь.G * Tube.Известь.SiO2 +
						 Tube.Известняк.ALFA * Tube.Известняк.G * Tube.Известняк.SiO2 + Tube.Футеровка.G * Tube.Футеровка.SiO2) +
						(Tube.Окатыши.ALFA * Tube.Окатыши.G * Tube.Окатыши.SiO2 + Tube.Руда.ALFA * Tube.Руда.G * Tube.Руда.SiO2 +
						 Tube.Окалина.ALFA * Tube.Окалина.G * Tube.Окалина.SiO2 + Tube.Песок.ALFA * Tube.Песок.G * Tube.Песок.SiO2);

					Tube.Шлак.B = TempLeft / TempRight;

					P2O5_calc();

					if (adaptCOMPAIR * 100 < 2)
					{
						GoodMeltsQuant = GoodMeltsQuant + 1;
						SummGshl = SummGshl + GshlSAVE[adaptROUND - 1];
						SummL = SummL + LSAVE[adaptROUND - 1];
						SummAlfaFe = SummAlfaFe + alfaFeSAVE[adaptROUND - 1];
						SummTeplFutLoss = SummTeplFutLoss + TeplFutLossSAVE[adaptROUND - 1];
					}
				}

				SaveMeltResults();
			}

			if (GoodMeltsQuant >= 10)
			{
				AdaptResultsSave();
				MessageBox.Show(
					string.Format("Адаптация системы расчета шихты успешно проведена. Использованы данные '{0}' плавок", GoodMeltsQuant),
					"Адаптация завершена");
			}
			else
			{
				MessageBox.Show(
					"Адаптация системы НЕ ПРОВЕДЕНА из-за неполноты или некорректности исходных данных. Результаты расчета по плавкам сохранены",
					"Внимание");
			}

			// Step11
		}

		private void AdaptResultsSave()
		{
			// TODO
		}

		private void SaveMeltResults()
		{
			// TODO
		}

		private void LoadMixAndOstShl()
		{
			Tube.ОставленныйШлак.Load();
			Tube.МиксерныйШлак.Load();
		}

		private void DetectBaseLenth()
		{
			var table = _meltMdb.Reader.FetchTable("adaptationdata");
			var rowcount = table.RowCount();

			for (var idx = 0; idx < rowcount; idx++)
			{
				var values = table.SelectRowRange(idx).Take(7).Select(value => value.ToDouble()).ToArray();
				if (values[3] * values[4] * values[5] * values[6] > 0)
				{
					BaseLenth = idx + 1;
				}
			}
		}

		private void LoadMeltData(int rowindex)
		{
			var table = _meltMdb.Reader.FetchTable("adaptationdata");
			var values = table.SelectRowRange(rowindex).Take(36).Select(value => value.ToDouble()).ToArray();

			var i = 1;
			Params.TAUprost = values[++i];
			Tube.Чугун.C = values[++i];
			Tube.Чугун.Si = values[++i];
			Tube.Чугун.Mn = values[++i];
			Tube.Чугун.P = values[++i];
			Tube.Чугун.S = values[++i];
			Tube.Чугун.T = values[++i];
			Tube.Чугун.G = values[++i];
			Tube.Лом.G = values[++i];
			Tube.Известь.G = values[++i];
			Tube.Известняк.G = values[++i];
			Tube.Доломит.G = values[++i];
			Tube.ВлажныйДоломит.G = values[++i];
			Tube.Имф.G = values[++i];
			Tube.Песок.G = values[++i];
			Tube.Кокс.G = values[++i];
			Tube.Окатыши.G = values[++i];
			Tube.Руда.G = values[++i];
			Tube.Окалина.G = values[++i];
			Tube.Агломерат.G = values[++i];
			Tube.Шпат.G = values[++i];
			Tube.Дутье.V = values[++i];
			Tube.Сталь.C = values[++i];
			Tube.Сталь.Mn = values[++i];
			Tube.Сталь.P = values[++i];
			Tube.Сталь.S = values[++i];
			Tube.Сталь.T = values[++i];
			Tube.Шлак.CaO = values[++i];
			Tube.Шлак.SiO2 = values[++i];
			Tube.Шлак.TOTALFeO = values[++i];
			Tube.Шлак.MgO = values[++i];
			Tube.Шлак.MnO = values[++i];
			AdaptationData.VArBlow = values[++i];
			AdaptationData.GDensing = values[++i];
		}

		private void LoadFromLoose(int sypuchIndex)
		{
			Tube.Известь.Load(sypuchIndex);
			Tube.Известняк.Load(sypuchIndex);
			Tube.Доломит.Load(sypuchIndex);
			Tube.ВлажныйДоломит.Load(sypuchIndex);
			Tube.Имф.Load(sypuchIndex);
			Tube.Кокс.Load(sypuchIndex);
			Tube.Песок.Load(sypuchIndex);
			Tube.Окатыши.Load(sypuchIndex);
			Tube.Руда.Load(sypuchIndex);
			Tube.Окалина.Load(sypuchIndex);
			Tube.Агломерат.Load(sypuchIndex);
			Tube.Шпат.Load(sypuchIndex);
		}

		private void LoadFromCountData(int selectedPlant)
		{
			try
			{
				var range = _paramsMdb.Reader.SelectRowRange("countdata", selectedPlant);
				Tube.Сталь.GYield = range[2].ToDouble();
				Tube.ОставленныйШлак.G = range[4].ToDouble();
				Params.StAndShlLoss = range[18].ToDouble();
				Tube.Футеровка.G = range[19].ToDouble() / Params.FutDurability;
				Tube.МиксерныйШлак.G = range[20].ToDouble();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void P2O5_calc()
		{
			Tube.Шлак.P2O5 = (
				Tube.Чугун.G * Tube.Чугун.P + Tube.Лом.G * Tube.Лом.P +
				Tube.Известь.ALFA * Tube.Известь.G * Tube.Известь.P2O5 * PConst +
				Tube.Известняк.ALFA * Tube.Известняк.G * Tube.Известняк.P2O5 * PConst + 
				Tube.Окалина.ALFA * Tube.Окалина.G * Tube.Окалина.P + Tube.Руда.ALFA * Tube.Руда.G * Tube.Руда.P +
				Tube.Футеровка.G * Tube.Футеровка.P2O5 * PConst +
				Tube.ОставленныйШлак.G * Tube.ОставленныйШлак.P2O5 * PConst +
				Tube.МиксерныйШлак.G * Tube.МиксерныйШлак.P2O5 * PConst + 
				Tube.Ферросплав.ALFA * Tube.Ферросплав.G * Tube.Ферросплав.P - 
				(Tube.Сталь.GYield / (1 - alfaFeSAVE[adaptROUND - 1] - Params.StAndShlLoss)) * Tube.Сталь.P) /
				(GshlSAVE[adaptROUND - 1] * PConst);
		}
	}
}
