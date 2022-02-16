﻿/*  CTRADER GURU

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/ctrader-guru

*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using cAlgo.API;
using cAlgo.API.Internals;

#region UPDATE : USING

using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;

#endregion

#region LICENZA : USING

using NM_CTG_Licenza;
using Button = cAlgo.API.Button;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

#endregion

namespace cAlgo
{
    /// <summary>
    /// Estensioni che rendono il codice più scorrevole con metodi non previsti dalla libreria cAlgo
    /// </summary>
    public static class Extensions
    {

        #region Enum

        /// <summary>
        /// Enumeratore per esporre il nome del colore nelle opzioni
        /// </summary>
        public enum ColorNameEnum
        {

            AliceBlue,
            AntiqueWhite,
            Aqua,
            Aquamarine,
            Azure,
            Beige,
            Bisque,
            Black,
            BlanchedAlmond,
            Blue,
            BlueViolet,
            Brown,
            BurlyWood,
            CadetBlue,
            Chartreuse,
            Chocolate,
            Coral,
            CornflowerBlue,
            Cornsilk,
            Crimson,
            Cyan,
            DarkBlue,
            DarkCyan,
            DarkGoldenrod,
            DarkGray,
            DarkGreen,
            DarkKhaki,
            DarkMagenta,
            DarkOliveGreen,
            DarkOrange,
            DarkOrchid,
            DarkRed,
            DarkSalmon,
            DarkSeaGreen,
            DarkSlateBlue,
            DarkSlateGray,
            DarkTurquoise,
            DarkViolet,
            DeepPink,
            DeepSkyBlue,
            DimGray,
            DodgerBlue,
            Firebrick,
            FloralWhite,
            ForestGreen,
            Fuchsia,
            Gainsboro,
            GhostWhite,
            Gold,
            Goldenrod,
            Gray,
            Green,
            GreenYellow,
            Honeydew,
            HotPink,
            IndianRed,
            Indigo,
            Ivory,
            Khaki,
            Lavender,
            LavenderBlush,
            LawnGreen,
            LemonChiffon,
            LightBlue,
            LightCoral,
            LightCyan,
            LightGoldenrodYellow,
            LightGray,
            LightGreen,
            LightPink,
            LightSalmon,
            LightSeaGreen,
            LightSkyBlue,
            LightSlateGray,
            LightSteelBlue,
            LightYellow,
            Lime,
            LimeGreen,
            Linen,
            Magenta,
            Maroon,
            MediumAquamarine,
            MediumBlue,
            MediumOrchid,
            MediumPurple,
            MediumSeaGreen,
            MediumSlateBlue,
            MediumSpringGreen,
            MediumTurquoise,
            MediumVioletRed,
            MidnightBlue,
            MintCream,
            MistyRose,
            Moccasin,
            NavajoWhite,
            Navy,
            OldLace,
            Olive,
            OliveDrab,
            Orange,
            OrangeRed,
            Orchid,
            PaleGoldenrod,
            PaleGreen,
            PaleTurquoise,
            PaleVioletRed,
            PapayaWhip,
            PeachPuff,
            Peru,
            Pink,
            Plum,
            PowderBlue,
            Purple,
            Red,
            RosyBrown,
            RoyalBlue,
            SaddleBrown,
            Salmon,
            SandyBrown,
            SeaGreen,
            SeaShell,
            Sienna,
            Silver,
            SkyBlue,
            SlateBlue,
            SlateGray,
            Snow,
            SpringGreen,
            SteelBlue,
            Tan,
            Teal,
            Thistle,
            Tomato,
            Transparent,
            Turquoise,
            Violet,
            Wheat,
            White,
            WhiteSmoke,
            Yellow,
            YellowGreen

        }

        /// <summary>
        /// Enumeratore per esporre nei parametri una scelta con menu a tendina
        /// </summary>
        public enum CapitalTo
        {

            Balance,
            Equity

        }

        /// <summary>
        /// Enumera la possibilità di scelta della direzione del profitto
        /// </summary>
        public enum ProfitDirection
        {

            All,
            Positive,
            Negative

        }

        public enum OpenTradeType
        {

            All,
            Buy,
            Sell

        }

        #endregion

        #region Class

        /// <summary>
        /// Classe per monitorare le posizioni di una specifica strategia
        /// </summary>
        public class Monitor
        {

            private Positions _allPositions = null;

            /// <summary>
            /// Standard per la raccolta di informazioni nel Monitor
            /// </summary>
            public class Information
            {

                public double TotalNetProfit = 0;
                public double MinVolumeInUnits = 0;
                public double MaxVolumeInUnits = 0;
                public double MidVolumeInUnits = 0;
                public int BuyPositions = 0;
                public int SellPositions = 0;
                public Position FirstPosition = null;
                public Position LastPosition = null;
                public double HighestHighAfterFirstOpen = 0;
                public double LowestLowAfterFirstOpen = 0;
                public double TotalLotsBuy = 0;
                public double TotalLotsSell = 0;
                public bool IAmInHedging = false;

            }

            /// <summary>
            /// Standard per l'interpretazione dell'orario in double
            /// </summary>
            public class PauseTimes
            {

                public double Over = 0;
                public double Under = 0;

            }

            /// <summary>
            /// Standard per la gestione del break even
            /// </summary>
            public class BreakEvenData
            {

                // --> In caso di operazioni multiple sarebbe bene evitare la gestione di tutte
                public bool OnlyFirst = false;
                public ProfitDirection ProfitDirection = ProfitDirection.All;
                public double Activation = 0;
                public int LimitBar = 0;
                public double Distance = 0;

            }

            /// <summary>
            /// Standard per la gestione del trailing
            /// </summary>
            public class TrailingData
            {

                // --> In caso di operazioni multiple sarebbe bene evitare la gestione di tutte
                public bool OnlyFirst = false;
                public bool ProActive = false;
                public double Activation = 0;
                public double Distance = 0;

            }

            /// <summary>
            /// Memorizza lo stato di apertura di una operazione nella Bar corrente
            /// </summary>
            public bool OpenedInThisBar = false;

            /// <summary>
            /// Memorizza lo stato di apertura di una operazione con il trigger corrente
            /// </summary>
            public bool OpenedInThisTrigger = false;

            /// <summary>
            /// Valore univoco che identifica la strategia
            /// </summary>
            public readonly string Label;

            /// <summary>
            /// Il Simbolo da monitorare in relazione alla Label
            /// </summary>
            public readonly Symbol Symbol;

            /// <summary>
            /// Le Bars con il quale la strategia si muove ed elabora le sue condizioni
            /// </summary>
            public readonly Bars Bars;

            /// <summary>
            /// Il riferimento temporale della pausa
            /// </summary>
            public readonly PauseTimes Pause;

            /// <summary>
            /// Le informazioni raccolte dopo la chiamata .Update()
            /// </summary>
            public Information Info { get; private set; }

            /// <summary>
            /// Le posizioni filtrate in base al simbolo e alla label
            /// </summary>
            public Position[] Positions { get; private set; }

            /// <summary>
            /// Monitor per la raccolta d'informazioni inerenti la strategia in corso
            /// </summary>
            public Monitor(string NewLabel, Symbol NewSymbol, Bars NewBars, Positions AllPositions, PauseTimes NewPause)
            {

                Label = NewLabel;
                Symbol = NewSymbol;
                Bars = NewBars;
                Pause = NewPause;

                Info = new Information();

                _allPositions = AllPositions;

                // --> Rendiamo sin da subito disponibili le informazioni
                Update(false, null, null, 0);

            }

            /// <summary>
            /// Filtra e rende disponibili le informazioni per la strategia monitorata. Eventualmente Chiude e gestisce le operazioni
            /// </summary>
            public Information Update(bool closeall, BreakEvenData breakevendata, TrailingData trailingdata, double SafeLoss, TradeType? filtertype = null)
            {

                // --> Raccolgo le informazioni che mi servono per avere il polso della strategia
                Positions = _allPositions.FindAll(Label, Symbol.Name);

                // --> Devo trascinarmi i vecchi dati prima di aggiornarli come massimali
                double highestHighAfterFirstOpen = (Positions.Length > 0) ? Info.HighestHighAfterFirstOpen : 0;
                double lowestLowAfterFirstOpen = (Positions.Length > 0) ? Info.LowestLowAfterFirstOpen : 0;

                // --> Resetto le informazioni
                Info = new Information 
                {

                    // --> Inizializzo con i vecchi dati
                    HighestHighAfterFirstOpen = highestHighAfterFirstOpen,
                    LowestLowAfterFirstOpen = lowestLowAfterFirstOpen

                };

                double tmpVolume = 0;

                foreach (Position position in Positions)
                {

                    // --> Per il trailing proactive e altre feature devo conoscere lo stato attuale
                    if (Info.HighestHighAfterFirstOpen == 0 || Symbol.Ask > Info.HighestHighAfterFirstOpen)
                        Info.HighestHighAfterFirstOpen = Symbol.Ask;
                    if (Info.LowestLowAfterFirstOpen == 0 || Symbol.Bid < Info.LowestLowAfterFirstOpen)
                        Info.LowestLowAfterFirstOpen = Symbol.Bid;

                    // --> Per prima cosa devo controllare se chiudere la posizione
                    if (closeall && (filtertype == null || position.TradeType == filtertype))
                    {

                        position.Close();
                        continue;

                    }

                    if (SafeLoss > 0 && position.StopLoss == null)
                    {

                        TradeResult result = position.ModifyStopLossPips(SafeLoss);

                        // --> Troppa voaltilità potrebbe portare a proporzioni e valori errati, comunque non andiamo oltre 
                        if (result.Error == ErrorCode.InvalidRequest || result.Error == ErrorCode.InvalidStopLossTakeProfit)
                        {

                            position.Close();

                        }

                        continue;

                    }

                    // --> Poi tocca al break even
                    if (breakevendata != null && (!breakevendata.OnlyFirst || Positions.Length == 1))
                        _checkBreakEven(position, breakevendata);

                    // --> Poi tocca al trailing
                    if (trailingdata != null && (!trailingdata.OnlyFirst || Positions.Length == 1))
                        _checkTrailing(position, trailingdata);

                    Info.TotalNetProfit += position.NetProfit;
                    tmpVolume += position.VolumeInUnits;

                    switch (position.TradeType)
                    {
                        case TradeType.Buy:

                            Info.BuyPositions++;
                            Info.TotalLotsBuy += position.Quantity;
                            break;

                        case TradeType.Sell:

                            Info.SellPositions++;
                            Info.TotalLotsSell += position.Quantity;
                            break;

                    }

                    if (Info.FirstPosition == null || position.EntryTime < Info.FirstPosition.EntryTime)
                        Info.FirstPosition = position;

                    if (Info.LastPosition == null || position.EntryTime > Info.LastPosition.EntryTime)
                        Info.LastPosition = position;

                    if (Info.MinVolumeInUnits == 0 || position.VolumeInUnits < Info.MinVolumeInUnits)
                        Info.MinVolumeInUnits = position.VolumeInUnits;

                    if (Info.MaxVolumeInUnits == 0 || position.VolumeInUnits > Info.MaxVolumeInUnits)
                        Info.MaxVolumeInUnits = position.VolumeInUnits;

                }

                // --> Restituisce una Exception Overflow di una operazione aritmetica, da approfondire
                //     Info.MidVolumeInUnits = Symbol.NormalizeVolumeInUnits(tmpVolume / Positions.Length,RoundingMode.ToNearest);
                Info.MidVolumeInUnits = Math.Round(tmpVolume / Positions.Length, 0);
                Info.IAmInHedging = (Positions.Length > 0 && Info.TotalLotsBuy == Info.TotalLotsSell);

                return Info;

            }

            /// <summary>
            /// Chiude tutte le posizioni del monitor
            /// </summary>
            public void CloseAllPositions(TradeType? filtertype = null)
            {

                Update(true, null, null, 0, filtertype);

            }

            /// <summary>
            /// Stabilisce se si è in GAP passando una certa distanza da misurare
            /// </summary>
            public bool InGAP(double distance)
            {

                return Symbol.DigitsToPips(Bars.LastGAP()) >= distance;

            }

            /// <summary>
            /// Controlla la fascia oraria per determinare se rientra in quella di pausa, utilizza dati double 
            /// perchè la ctrader non permette di esporre dati time, da aggiornare non appena la ctrader lo permette
            /// </summary>
            /// <returns>Conferma la presenza di una fascia oraria in pausa</returns>
            public bool InPause(DateTime timeserver)
            {

                // -->> Poichè si utilizzano dati double per esporre i parametri dobbiamo utilizzare meccanismi per tradurre l'orario
                string nowHour = (timeserver.Hour < 10) ? string.Format("0{0}", timeserver.Hour) : string.Format("{0}", timeserver.Hour);
                string nowMinute = (timeserver.Minute < 10) ? string.Format("0{0}", timeserver.Minute) : string.Format("{0}", timeserver.Minute);

                // --> Stabilisco il momento di controllo in formato double
                double adesso = Convert.ToDouble(string.Format("{0},{1}", nowHour, nowMinute));

                // --> Confronto elementare per rendere comprensibile la logica
                if (Pause.Over < Pause.Under && adesso >= Pause.Over && adesso <= Pause.Under)
                {

                    return true;

                }
                else if (Pause.Over > Pause.Under && ((adesso >= Pause.Over && adesso <= 23.59) || adesso <= Pause.Under))
                {

                    return true;

                }

                return false;

            }

            /// <summary>
            /// Controlla ed effettua la modifica in break-even se le condizioni le permettono
            /// </summary>
            private void _checkBreakEven(Position position, BreakEvenData breakevendata)
            {

                if (breakevendata == null || breakevendata.Activation == 0)
                    return;

                double activation = Symbol.PipsToDigits(breakevendata.Activation);

                int currentMinutes = Bars.TimeFrame.ToMinutes();
                DateTime limitTime = position.EntryTime.AddMinutes(currentMinutes * breakevendata.LimitBar);
                bool limitActivation = (breakevendata.LimitBar > 0 && Bars.Last(0).OpenTime >= limitTime);

                double distance = Symbol.PipsToDigits(breakevendata.Distance);

                switch (position.TradeType)
                {

                    case TradeType.Buy:

                        double breakevenpointbuy = Math.Round(position.EntryPrice + distance, Symbol.Digits);

                        if (position.StopLoss == breakevenpointbuy || position.TakeProfit == breakevenpointbuy)
                            break;

                        if ((Symbol.Bid > breakevenpointbuy) && (limitActivation || (breakevendata.ProfitDirection != ProfitDirection.Negative && (Symbol.Bid >= (position.EntryPrice + activation)))) && (position.StopLoss == null || position.StopLoss < breakevenpointbuy))
                        {

                            position.ModifyStopLossPrice(breakevenpointbuy);

                        }
                        else if ((Symbol.Ask < breakevenpointbuy) && (limitActivation || (breakevendata.ProfitDirection != ProfitDirection.Positive && (Symbol.Bid <= (position.EntryPrice - activation)))) && (position.TakeProfit == null || position.TakeProfit > breakevenpointbuy))
                        {

                            position.ModifyTakeProfitPrice(breakevenpointbuy);

                        }

                        break;

                    case TradeType.Sell:

                        double breakevenpointsell = Math.Round(position.EntryPrice - distance, Symbol.Digits);

                        if (position.StopLoss == breakevenpointsell || position.TakeProfit == breakevenpointsell)
                            break;

                        if ((Symbol.Bid < breakevenpointsell) && (limitActivation || (breakevendata.ProfitDirection != ProfitDirection.Negative && (Symbol.Ask <= (position.EntryPrice - activation)))) && (position.StopLoss == null || position.StopLoss > breakevenpointsell))
                        {

                            position.ModifyStopLossPrice(breakevenpointsell);

                        }
                        else if ((Symbol.Ask > breakevenpointsell) && (limitActivation || (breakevendata.ProfitDirection != ProfitDirection.Positive && (Symbol.Ask >= (position.EntryPrice + activation)))) && (position.TakeProfit == null || position.TakeProfit < breakevenpointsell))
                        {

                            position.ModifyTakeProfitPrice(breakevenpointsell);

                        }

                        break;

                }

            }


            /// <summary>
            /// Controlla ed effettua la modifica in trailing se le condizioni le permettono
            /// </summary>
            private void _checkTrailing(Position position, TrailingData trailingdata)
            {

                if (trailingdata == null || trailingdata.Activation == 0 || trailingdata.Distance == 0)
                    return;

                double trailing = 0;
                double distance = Symbol.PipsToDigits(trailingdata.Distance);
                double activation = Symbol.PipsToDigits(trailingdata.Activation);

                switch (position.TradeType)
                {

                    case TradeType.Buy:

                        trailing = Math.Round(Symbol.Bid - distance, Symbol.Digits);

                        if (position.StopLoss == trailing || position.TakeProfit == trailing)
                            break;

                        if ((Symbol.Bid >= (position.EntryPrice + activation)) && (position.StopLoss == null || position.StopLoss < trailing))
                        {

                            position.ModifyStopLossPrice(trailing);

                        }
                        else if (trailingdata.ProActive && Info.HighestHighAfterFirstOpen > 0 && position.StopLoss != null && position.StopLoss > 0)
                        {

                            // --> Devo determinare se è partita l'attivazione
                            double activationprice = position.EntryPrice + activation;
                            double firsttrailing = Math.Round(activationprice - distance, Symbol.Digits);

                            // --> Partito il trailing? Sono in retrocessione ?
                            if (position.StopLoss >= firsttrailing)
                            {

                                double limitpriceup = Info.HighestHighAfterFirstOpen;
                                double limitpricedw = Math.Round(Info.HighestHighAfterFirstOpen - distance, Symbol.Digits);

                                double k = Math.Round(limitpriceup - Symbol.Ask, Symbol.Digits);

                                double newtrailing = Math.Round(limitpricedw + k, Symbol.Digits);

                                if (position.StopLoss == newtrailing || position.TakeProfit == newtrailing)
                                    break;

                                if (position.StopLoss < newtrailing)
                                    position.ModifyStopLossPrice(newtrailing);

                            }

                        }

                        break;

                    case TradeType.Sell:

                        trailing = Math.Round(Symbol.Ask + Symbol.PipsToDigits(trailingdata.Distance), Symbol.Digits);

                        if (position.StopLoss == trailing || position.TakeProfit == trailing)
                            break;

                        if ((Symbol.Ask <= (position.EntryPrice - Symbol.PipsToDigits(trailingdata.Activation))) && (position.StopLoss == null || position.StopLoss > trailing))
                        {

                            position.ModifyStopLossPrice(trailing);

                        }
                        else if (trailingdata.ProActive && Info.LowestLowAfterFirstOpen > 0 && position.StopLoss != null && position.StopLoss > 0)
                        {

                            // --> Devo determinare se è partita l'attivazione
                            double activationprice = position.EntryPrice - activation;
                            double firsttrailing = Math.Round(activationprice + distance, Symbol.Digits);

                            // --> Partito il trailing? Sono in retrocessione ?
                            if (position.StopLoss <= firsttrailing)
                            {

                                double limitpriceup = Math.Round(Info.LowestLowAfterFirstOpen + distance, Symbol.Digits);
                                double limitpricedw = Info.LowestLowAfterFirstOpen;

                                double k = Math.Round(Symbol.Bid - limitpricedw, Symbol.Digits);

                                double newtrailing = Math.Round(limitpriceup - k, Symbol.Digits);

                                if (position.StopLoss == newtrailing || position.TakeProfit == newtrailing)
                                    break;

                                if (position.StopLoss > newtrailing)
                                    position.ModifyStopLossPrice(newtrailing);

                            }

                        }

                        break;

                }

            }

        }

        /// <summary>
        /// Classe per gestire il dimensionamento delle size
        /// </summary>
        public class MonenyManagement
        {

            private readonly double _minSize = 0.01;
            private double _percentage = 0;
            private double _fixedSize = 0;
            private double _pipToCalc = 30;

            // --> Riferimenti agli oggetti esterni utili per il calcolo
            private IAccount _account = null;
            public readonly Symbol Symbol;

            /// <summary>
            /// Il capitale da utilizzare per il calcolo
            /// </summary>
            public CapitalTo CapitalType = CapitalTo.Balance;

            /// <summary>
            /// La percentuale di rischio che si vuole investire
            /// </summary>
            public double Percentage
            {

                get { return _percentage; }


                set { _percentage = (value > 0 && value <= 100) ? value : 0; }
            }

            /// <summary>
            /// La size fissa da utilizzare, bypassa tutti i parametri di calcolo
            /// </summary>
            public double FixedSize
            {

                get { return _fixedSize; }



                set { _fixedSize = (value >= _minSize) ? value : 0; }
            }


            /// <summary>
            /// La distanza massima dall'ingresso con il quale calcolare le size
            /// </summary>
            public double PipToCalc
            {

                get { return _pipToCalc; }

                set { _pipToCalc = (value > 0) ? value : 100; }
            }


            /// <summary>
            /// Il capitale effettivo sul quale calcolare il rischio
            /// </summary>
            public double Capital
            {

                get
                {

                    switch (CapitalType)
                    {

                        case CapitalTo.Equity:

                            return _account.Equity;
                        default:


                            return _account.Balance;

                    }

                }
            }



            // --> Costruttore
            public MonenyManagement(IAccount NewAccount, CapitalTo NewCapitalTo, double NewPercentage, double NewFixedSize, double NewPipToCalc, Symbol NewSymbol)
            {

                _account = NewAccount;

                Symbol = NewSymbol;

                CapitalType = NewCapitalTo;
                Percentage = NewPercentage;
                FixedSize = NewFixedSize;
                PipToCalc = NewPipToCalc;

            }

            /// <summary>
            /// Restituisce il numero di lotti in formato 0.01
            /// </summary>
            public double GetLotSize()
            {

                // --> Hodeciso di usare una size fissa
                if (FixedSize > 0)
                    return FixedSize;

                // --> La percentuale di rischio in denaro
                double moneyrisk = Capital / 100 * Percentage;

                // --> Traduco lo stoploss o il suo riferimento in double
                double sl_double = PipToCalc * Symbol.PipSize;

                // --> In formato 0.01 = microlotto double lots = Math.Round(Symbol.VolumeInUnitsToQuantity(moneyrisk / ((sl_double * Symbol.TickValue) / Symbol.TickSize)), 2);
                // --> In formato volume 1K = 1000 Math.Round((moneyrisk / ((sl_double * Symbol.TickValue) / Symbol.TickSize)), 2);
                double lots = Math.Round(Symbol.VolumeInUnitsToQuantity(moneyrisk / ((sl_double * Symbol.TickValue) / Symbol.TickSize)), 2);

                if (lots < _minSize)
                    return _minSize;

                return lots;

            }

        }

        #endregion

        #region Helper

        /// <summary>
        /// Restituisce il colore corrispondente a partire dal nome
        /// </summary>
        /// <returns>Il colore corrispondente</returns>
        public static API.Color ColorFromEnum(ColorNameEnum colorName)
        {

            return API.Color.FromName(colorName.ToString("G"));

        }

        #endregion

        #region Bars

        /// <summary>
        /// Si ottiene l'indice della candela partendo dal suo orario di apertura
        /// </summary>
        /// <param name="MyTime">La data e l'ora di apertura della candela</param>
        /// <returns></returns>
        public static int GetIndexByDate(this Bars thisBars, DateTime thisTime)
        {

            for (int i = thisBars.ClosePrices.Count - 1; i >= 0; i--)
            {

                if (thisTime == thisBars.OpenTimes[i])
                    return i;

            }

            return -1;

        }

        public static double LastGAP(this Bars thisBars)
        {

            double K = 0;

            if (thisBars.ClosePrices.Last(1) > thisBars.OpenPrices.LastValue)
            {

                K = Math.Round(thisBars.ClosePrices.Last(1) - thisBars.OpenPrices.LastValue, 5);

            }
            else if (thisBars.ClosePrices.Last(1) < thisBars.OpenPrices.LastValue)
            {

                K = Math.Round(thisBars.OpenPrices.LastValue - thisBars.ClosePrices.Last(1), 5);

            }

            return K;

        }

        #endregion

        #region Bar

        /// <summary>
        /// Misura la grandezza di una candela, tenendo conto della sua direzione
        /// </summary>
        /// <returns>Il corpo della candela, valore uguale o superiore a zero</returns>
        public static double Body(this Bar thisBar)
        {

            return thisBar.IsBullish() ? thisBar.Close - thisBar.Open : thisBar.Open - thisBar.Close;


        }

        /// <summary>
        /// Verifica la direzione rialzista di una candela
        /// </summary>
        /// <returns>True se la candela è rialzista</returns>        
        public static bool IsBullish(this Bar thisBar)
        {

            return thisBar.Close > thisBar.Open;

        }

        /// <summary>
        /// Verifica la direzione ribassista di una candela
        /// </summary>
        /// <returns>True se la candela è ribassista</returns>        
        public static bool IsBearish(this Bar thisBar)
        {

            return thisBar.Close < thisBar.Open;

        }

        /// <summary>
        /// Verifica se una candela ha un open uguale al close
        /// </summary>
        /// <returns>True se la candela è una doji con Open e Close uguali</returns>        
        public static bool IsDoji(this Bar thisBar)
        {

            return thisBar.Close == thisBar.Open;

        }

        #endregion

        #region Symbol

        /// <summary>
        /// Converte il numero di pips corrente da digits a double
        /// </summary>
        /// <param name="Pips">Il numero di pips nel formato Digits</param>
        /// <returns></returns>
        public static double DigitsToPips(this Symbol thisSymbol, double Pips)
        {

            return Math.Round(Pips / thisSymbol.PipSize, 2);

        }

        /// <summary>
        /// Converte il numero di pips corrente da double a digits
        /// </summary>
        /// <param name="Pips">Il numero di pips nel formato Double (2)</param>
        /// <returns></returns>
        public static double PipsToDigits(this Symbol thisSymbol, double Pips)
        {

            return Math.Round(Pips * thisSymbol.PipSize, thisSymbol.Digits);

        }

        public static double RealSpread(this Symbol thisSymbol)
        {

            return Math.Round(thisSymbol.Spread / thisSymbol.PipSize, 2);

        }

        #endregion

        #region TimeFrame

        /// <summary>
        /// Restituisce in minuti il timeframe corrente
        /// </summary>
        public static int ToMinutes(this TimeFrame thisTimeFrame)
        {

            if (thisTimeFrame == TimeFrame.Daily)
                return 60 * 24;
            if (thisTimeFrame == TimeFrame.Day2)
                return 60 * 24 * 2;
            if (thisTimeFrame == TimeFrame.Day3)
                return 60 * 24 * 3;
            if (thisTimeFrame == TimeFrame.Hour)
                return 60;
            if (thisTimeFrame == TimeFrame.Hour12)
                return 60 * 12;
            if (thisTimeFrame == TimeFrame.Hour2)
                return 60 * 2;
            if (thisTimeFrame == TimeFrame.Hour3)
                return 60 * 3;
            if (thisTimeFrame == TimeFrame.Hour4)
                return 60 * 4;
            if (thisTimeFrame == TimeFrame.Hour6)
                return 60 * 6;
            if (thisTimeFrame == TimeFrame.Hour8)
                return 60 * 8;
            if (thisTimeFrame == TimeFrame.Minute)
                return 1;
            if (thisTimeFrame == TimeFrame.Minute10)
                return 10;
            if (thisTimeFrame == TimeFrame.Minute15)
                return 15;
            if (thisTimeFrame == TimeFrame.Minute2)
                return 2;
            if (thisTimeFrame == TimeFrame.Minute20)
                return 20;
            if (thisTimeFrame == TimeFrame.Minute3)
                return 3;
            if (thisTimeFrame == TimeFrame.Minute30)
                return 30;
            if (thisTimeFrame == TimeFrame.Minute4)
                return 4;
            if (thisTimeFrame == TimeFrame.Minute45)
                return 45;
            if (thisTimeFrame == TimeFrame.Minute5)
                return 5;
            if (thisTimeFrame == TimeFrame.Minute6)
                return 6;
            if (thisTimeFrame == TimeFrame.Minute7)
                return 7;
            if (thisTimeFrame == TimeFrame.Minute8)
                return 8;
            if (thisTimeFrame == TimeFrame.Minute9)
                return 9;
            if (thisTimeFrame == TimeFrame.Monthly)
                return 60 * 24 * 30;
            if (thisTimeFrame == TimeFrame.Weekly)
                return 60 * 24 * 7;

            return 0;

        }

        #endregion

    }

}

namespace cAlgo.Robots
{

    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class PriceActionAdvisor : Robot
    {

        #region Enums

        public enum MyTradeType
        {

            Disabled,
            Buy,
            Sell

        }

        public enum ProtectionType
        {

            Disabled,
            OnlyFirst,
            All

        }

        public enum LoopType
        {

            OnBar,
            OnTick

        }

        public enum StopMode
        {

            Standard,
            Auto

        }

        public enum DrawDownMode
        {
            Disabled,
            Close,
            Hedging

        }

        public enum AccCapital
        {
            Balance,
            FreeMargin,
            Equity

        }

        #endregion

        #region Identity
        /// <summary>
        /// ID prodotto, identificativo, viene fornito da ctrader.guru, 60886 è il riferimento del template in uso
        /// </summary>
        public const int ID = 213056;

        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione
        /// </summary>
        public const string NAME = "Price Action Advisor";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.4";

        #endregion

        #region UPDATE : VARIABILI

        private const string PRODUCTPAGE = "https://ctrader.guru/shop/tools/price-action-advisor/";
        private const string LICENSEPAGE = "https://ctrader.guru/license/";

        #endregion

        #region LICENZA : VARIABILI

        string productName = NAME;
        readonly string endpoint = "https://ctrader.guru/_checkpoint_/";

        DateTime licenzaExpire;
        CL_CTG_Licenza licenza = null;
        CL_CTG_Licenza.LicenzaInfo licenzaInfo = null;
        bool exitoncalculate = false;
        private ControlBase DrawingDialog = null;

        #endregion
        
        #region Params

        /// <summary>
        /// Riferimenti del prodotto
        /// </summary>
        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = PRODUCTPAGE)]
        public string ProductInfo { get; set; }

        /// <summary>
        /// Label che contraddistingue una operazione
        /// </summary>
        [Parameter("Label ( Magic Name )", Group = "Identity", DefaultValue = NAME)]
        public string MyLabel { get; set; }

        /// <summary>
        /// Informazioni sul default preset
        /// </summary>
        [Parameter("Preset information", Group = "Identity", DefaultValue = "EURUSD 1H")]
        public string PresetInfo { get; set; }

        [Parameter("Period (bars)", Group = "Average", DefaultValue = 50, MinValue = 2)]
        public int AVGperiod { get; set; }

        [Parameter("Minimum (pips; zero = all)", Group = "Average", DefaultValue = 0, MinValue = 0, Step = 0.1)]
        public double AVGminimum { get; set; }

        [Parameter("Trading ?", Group = "Trading", DefaultValue = true)]
        public bool TradingEnabled { get; set; }

        [Parameter("Recovery ?", Group = "Trading", DefaultValue = true)]
        public bool RecoveryEnabled { get; set; }

        [Parameter("Activated ?", Group = "Alerts", DefaultValue = true)]
        public bool AlertsEnabled { get; set; }

        /*
        [Parameter("Max Cross Coworking (zero disabled)", Group = "Strategy", DefaultValue = 0, MinValue = 0)]
        public int MaxCross { get; set; }
        */
        public int MaxCross = 0;

        /*
        [Parameter("Loop", Group = "Strategy", DefaultValue = LoopType.OnBar)]
        public LoopType MyLoopType { get; set; }
        */
        public LoopType MyLoopType = LoopType.OnTick;

        /*
        [Parameter("Open Trade Type", Group = "Strategy", DefaultValue = Extensions.OpenTradeType.All)]
        public Extensions.OpenTradeType MyOpenTradeType { get; set; }
        */
        public Extensions.OpenTradeType MyOpenTradeType = Extensions.OpenTradeType.All;

        //[Parameter("Stop", Group = "Strategy", DefaultValue = StopMode.Auto)]
        //public StopMode MyStopType { get; set; }
        public StopMode MyStopType = StopMode.Standard;

        //[Parameter("Safe StopLoss", Group = "Strategy", DefaultValue = 10, MinValue = 0, Step = 0.1)]
        //public double StopLevel { get; set; }
        double StopLevel = 5;

        //[Parameter("Boring Close (bars, zero disabled)", Group = "Strategy", DefaultValue = 0, MinValue = 0)]
        //public int Boring { get; set; }
        public int Boring = 0;

        //[Parameter("Break Even", Group = "Strategy", DefaultValue = ProtectionType.OnlyFirst)]
        //public ProtectionType BreakEvenProtectionType { get; set; }
        public ProtectionType BreakEvenProtectionType = ProtectionType.Disabled;

        //[Parameter("Trailing", Group = "Strategy", DefaultValue = ProtectionType.OnlyFirst)]
        //public ProtectionType TrailingProtectionType { get; set; }
        public ProtectionType TrailingProtectionType = ProtectionType.Disabled;

        //[Parameter("Drawdown", Group = "Strategy", DefaultValue = DrawDownMode.Disabled)]
        //public DrawDownMode ddMode { get; set; }
        public DrawDownMode ddMode = DrawDownMode.Disabled;

        //[Parameter("Money Target (%, zero disabled)", Group = "Strategy", DefaultValue = 0, MinValue = 0, Step = 0.1)]
        //public double MoneyTargetPercentage { get; set; }
        public double MoneyTargetPercentage = 0;

        //[Parameter("Money Target Minimum Trades", Group = "Strategy", DefaultValue = 1, MinValue = 1, Step = 1)]
        //public int MoneyTargetTrades { get; set; }
        public int MoneyTargetTrades = 1;

        //[Parameter("Recovery Multiplier", Group = "Strategy", DefaultValue = 1, MinValue = 1, Step = 0.5)]
        //public double RecoveryMultiplier { get; set; }
        public double RecoveryMultiplier = 1;

        //[Parameter("Slippage (pips)", Group = "Strategy", DefaultValue = 2.0, MinValue = 0.5, Step = 0.1)]
        //public double SLIPPAGE { get; set; }
        public double SLIPPAGE = 2;

        [Parameter("Max Spread allowed", Group = "Filters", DefaultValue = 1.5, MinValue = 0.1, Step = 0.1)]
        public double SpreadToTrigger { get; set; }

        [Parameter("Pause over this time", Group = "Filters", DefaultValue = 0, MinValue = 0, MaxValue = 23.59)]
        public double PauseOver { get; set; }

        [Parameter("Pause under this time", Group = "Filters", DefaultValue = 0, MinValue = 0, MaxValue = 23.59)]
        public double PauseUnder { get; set; }

        [Parameter("Max GAP Allowed (pips)", Group = "Filters", DefaultValue = 3, MinValue = 0, Step = 0.01)]
        public double GAP { get; set; }

        //[Parameter("Max Number of Trades", Group = "Filters", DefaultValue = 1, MinValue = 1, Step = 1)]
        //public int MaxTrades { get; set; }
        public int MaxTrades = 1;

        //[Parameter("Hedging Opportunity ?", Group = "Filters", DefaultValue = false)]
        //public bool HedgingOpportunity { get; set; }
        public bool HedgingOpportunity = false;

        [Parameter("Fixed Lots", Group = "Money Management", DefaultValue = 0, MinValue = 0, Step = 0.01)]
        public double FixedLots { get; set; }

        [Parameter("Capital", Group = "Money Management", DefaultValue = Extensions.CapitalTo.Balance)]
        public Extensions.CapitalTo MyCapital { get; set; }

        [Parameter("% Risk", Group = "Money Management", DefaultValue = 1, MinValue = 0.1, Step = 0.1)]
        public double MyRisk { get; set; }

        [Parameter("Pips To Calculate ( if no stoploss, empty = '100' )", Group = "Money Management", DefaultValue = 100, MinValue = 0, Step = 0.1)]
        public double FakeSL { get; set; }

        //[Parameter("Stop Loss (pips)", Group = "Standard Stop", DefaultValue = 0, MinValue = 0, Step = 0.1)]
        //public double SL { get; set; }
        public double SL = 0;

        //[Parameter("Take Profit (pips)", Group = "Standard Stop", DefaultValue = 0, MinValue = 0, Step = 0.1)]
        //public double TP { get; set; }
        public double TP = 0;

        //[Parameter("Period", Group = "Auto Stop", DefaultValue = 5, MinValue = 1, Step = 1)]
        //public int AutoStopPeriod { get; set; }
        public int AutoStopPeriod = 5;

        //[Parameter("Minimum (pips)", Group = "Auto Stop", DefaultValue = 10, MinValue = 1, Step = 0.1)]
        //public double AutoMinPips { get; set; }
        public double AutoMinPips = 10;

        //[Parameter("K (pips)", Group = "Auto Stop", DefaultValue = 3, MinValue = 0, Step = 0.1)]
        //public double KPips { get; set; }
        public double KPips = 3;

        //[Parameter("R:R (zero disable take profit)", Group = "Auto Stop", DefaultValue = 0, MinValue = 0, Step = 1)]
        //public double AutoStopRR { get; set; }
        public double AutoStopRR = 0;

        //[Parameter("Profit Direction ?", Group = "Break Even", DefaultValue = Extensions.ProfitDirection.All)]
        //public Extensions.ProfitDirection BreakEvenProfitDirection { get; set; }
        public Extensions.ProfitDirection BreakEvenProfitDirection = Extensions.ProfitDirection.All;

        //[Parameter("Activation (pips)", Group = "Break Even", DefaultValue = 30, MinValue = 1, Step = 0.1)]
        //public double BreakEvenActivation { get; set; }
        public double BreakEvenActivation = 30;

        //[Parameter("Activation Limit (bars)", Group = "Break Even", DefaultValue = 11, MinValue = 0, Step = 1)]
        //public int BreakEvenLimitBars { get; set; }
        public int BreakEvenLimitBars = 11;

        //[Parameter("Distance (pips, move Stop Loss)", Group = "Break Even", DefaultValue = 1.5, Step = 0.1)]
        //public double BreakEvenDistance { get; set; }
        public double BreakEvenDistance = 1.1;

        //[Parameter("Activation (pips)", Group = "Trailing", DefaultValue = 40, MinValue = 1, Step = 0.1)]
        //public double TrailingActivation { get; set; }
        public double TrailingActivation = 40;

        //[Parameter("Distance (pips, move Stop Loss)", Group = "Trailing", DefaultValue = 30, MinValue = 1, Step = 0.1)]
        //public double TrailingDistance { get; set; }
        public double TrailingDistance = 30;

        //[Parameter("ProActive ?", Group = "Trailing", DefaultValue = false)]
        //public bool TrailingProactive { get; set; }
        public bool TrailingProactive = false;

        //[Parameter("Capital", Group = "Drawdown", DefaultValue = AccCapital.Balance)]
        //public AccCapital accCapital { get; set; }
        public AccCapital accCapital = AccCapital.Balance;

        //[Parameter("Max %", Group = "Drawdown", DefaultValue = 20, MinValue = 0, MaxValue = 100, Step = 0.1)]
        //public double ddPercentage { get; set; }
        public double ddPercentage = 20;

        //[Parameter("Open Position On Start", Group = "Debug", DefaultValue = MyTradeType.Disabled)]
        //public MyTradeType OpenOnStart { get; set; }
        public MyTradeType OpenOnStart = MyTradeType.Disabled;

        //[Parameter("Verbose ?", Group = "Debug", DefaultValue = true)]
        //public bool DebugVerbose { get; set; }
        public bool DebugVerbose = false;

        [Parameter("Color Text", Group = "Styles", DefaultValue = Extensions.ColorNameEnum.DodgerBlue)]
        public Extensions.ColorNameEnum TextColor { get; set; }

        [Parameter("Size Text", Group = "Styles", DefaultValue = 12)]
        public int TextSize { get; set; }

        #endregion

        #region Property

        Extensions.Monitor.PauseTimes Pause1;
        Extensions.Monitor Monitor1;
        Extensions.MonenyManagement MonenyManagement1;
        Extensions.Monitor.BreakEvenData BreakEvenData1;
        Extensions.Monitor.TrailingData TrailingData1;

        private double SafeLoss = 0;
        bool CanDraw;

        double AVG_Current = 0;
        bool IsAllowed = false;
        bool IsInTrigger = false;
        double AVG_Candle = 0;
        bool IsAlertSend = false;
        int ConsecutiveLoss = 0;

        #endregion

        #region cBot Events

        /// <summary>
        /// Evento generato quando viene avviato il cBot
        /// </summary>
        protected override void OnStart()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);

            #region LICENZA : INIT CHECK

            CL_CTG_Licenza.LicenzaConfig licConfig = new CL_CTG_Licenza.LicenzaConfig
            {
                AccountBroker = Account.BrokerName,
                AcconuntNumber = Account.Number.ToString()
            };

            licenza = new CL_CTG_Licenza(endpoint, licConfig, productName);

            _checkLicense();

            if (exitoncalculate)
                return;

            #endregion

            #region UPDATE : INIT CHECK

            _checkProductUpdate();

            #endregion


            SafeLoss = (MyStopType == StopMode.Auto || SL > 0) ? StopLevel : 0;
            CanDraw = (RunningMode == RunningMode.RealTime || RunningMode == RunningMode.VisualBacktesting);

            _updateAverage();

            _updateInformation();

            // --> Determino il range di pausa
            Pause1 = new Extensions.Monitor.PauseTimes 
            {

                Over = PauseOver,
                Under = PauseUnder

            };

            // --> Inizializzo il Monitor
            Monitor1 = new Extensions.Monitor(MyLabel, Symbol, Bars, Positions, Pause1);

            // --> Inizializzo il MoneyManagement
            MonenyManagement1 = new Extensions.MonenyManagement(Account, MyCapital, MyRisk, FixedLots, SL > 0 ? SL : FakeSL, Symbol);

            // --> Inizializzo i dati per la gestione del breakeven
            BreakEvenData1 = new Extensions.Monitor.BreakEvenData 
            {

                OnlyFirst = BreakEvenProtectionType == ProtectionType.OnlyFirst,
                ProfitDirection = BreakEvenProfitDirection,
                Activation = (BreakEvenProtectionType != ProtectionType.Disabled) ? BreakEvenActivation : 0,
                LimitBar = BreakEvenLimitBars,
                Distance = BreakEvenDistance

            };

            // --> Inizializzo i dati per la gestione del Trailing
            TrailingData1 = new Extensions.Monitor.TrailingData 
            {

                OnlyFirst = TrailingProtectionType == ProtectionType.OnlyFirst,
                ProActive = TrailingProactive,
                Activation = (TrailingProtectionType != ProtectionType.Disabled) ? TrailingActivation : 0,
                Distance = TrailingDistance

            };

            // --> Osservo le aperture per operazioni comuni
            Positions.Opened += _onOpenPositions;
            Positions.Closed += _onPositionsClosed;

            // --> Effettuo un test di apertura per verificare il funzionamento del sistema
            if (OpenOnStart != MyTradeType.Disabled)
                _test((OpenOnStart == MyTradeType.Buy) ? TradeType.Buy : TradeType.Sell, Monitor1, MonenyManagement1, MyLabel);

        }

        /// <summary>
        /// Evento generato quando viene fermato il cBot
        /// </summary>
        protected override void OnStop()
        {

            // --> Meglio eliminare l'handler, non dovrebbe servire ma non si sa mai
            Positions.Opened -= _onOpenPositions;

        }

        /// <summary>
        /// Evento generato ad ogni cambio candela
        /// </summary>
        protected override void OnBar()
        {

            IsAlertSend = false;

            // --> Aggiorno il valore di AVG_Current
            _updateAverage();

            // --> Resetto il flag del controllo candela
            Monitor1.OpenedInThisBar = false;

            // --> Eseguo il loop solo se desidero farlo ad ogni cambio candela
            if (MyLoopType == LoopType.OnBar)
                _loop(Monitor1, MonenyManagement1, BreakEvenData1, TrailingData1);

        }

        /// <summary>
        /// Evento generato a ogni tick
        /// </summary>
        protected override void OnTick()
        {

            #region LICENZA : LOOP CHECK                       

            if (RunningMode == RunningMode.RealTime)
            {

                if (exitoncalculate)
                {

                    if (DrawingDialog != null && !DrawingDialog.IsVisible)
                    {

                        Stop();

                    }
                    else
                    {

                        _createButtonLogin();

                    }

                    return;

                }
                else if (licenzaExpire != null && licenzaInfo.Expire.CompareTo("*") != 0 && Monitor1.Positions.Length == 0)
                {

                    if (DateTime.Compare(licenzaExpire, Server.Time) > 0)
                    {

                        // --> TODO

                    }
                    else
                    {

                        exitoncalculate = true;

                        Print("Expired (" + licenzaExpire + ")" + " (server : " + Server.Time.ToString() + ")");

                        return;

                    }

                }

            }

            #endregion

            // --> Devo comunque controllare i breakeven e altro nel tick
            Monitor1.Update(_checkClosePositions(Monitor1), Monitor1.Info.IAmInHedging ? null : BreakEvenData1, Monitor1.Info.IAmInHedging ? null : TrailingData1, SafeLoss, null);

            // --> Devo comunque controllare e stampare a video la logica
            _updateInformation();

            // --> Controllo il drawdown o se sono di nuovo in hedging
            if (Monitor1.Info.IAmInHedging || _checkDrawdownMode(Monitor1))
                return;

            // --> Controllo se sono passate troppe candele e voglio chiudere, minimo 2 posizioni
            if (Boring > 0 && Monitor1.Positions.Length >= 2 && Monitor1.Info.TotalNetProfit > 0)
            {

                // --> Ricavo l'indice della prima posizione
                int currentIndex = Bars.Count - 1;
                int indexFirstTrade = Bars.OpenTimes.GetIndexByTime(Monitor1.Info.FirstPosition.EntryTime);

                if ((currentIndex - indexFirstTrade) >= Boring)
                {

                    Monitor1.CloseAllPositions();
                    _log("Closed for Boring Bars ");
                    return;
                }

            }

            // --> Eseguo il loop solo se desidero farlo ad ogni Tick
            if (MyLoopType == LoopType.OnTick)
                _loop(Monitor1, MonenyManagement1, BreakEvenData1, TrailingData1);

        }

        private void _onPositionsClosed(PositionClosedEventArgs args)
        {

            if (!RecoveryEnabled)
                return;

            var position = args.Position;

            if (position.Label != MyLabel || position.SymbolName != SymbolName)
                return;

            if (position.NetProfit < 0)
            {

                ConsecutiveLoss++;

                if (ConsecutiveLoss <= 1)
                {

                    TradeType reversed = (position.TradeType == TradeType.Sell) ? TradeType.Buy : TradeType.Sell;

                    double realSL = Math.Round(Symbol.DigitsToPips(Math.Abs((double)position.StopLoss - position.EntryPrice)), 2);

                    var result = ExecuteMarketOrder(reversed, SymbolName, Symbol.QuantityToVolumeInUnits(Math.Round(position.Quantity, 2)), MyLabel, realSL, realSL);

                    if (result.Error == ErrorCode.NoMoney)
                    {

                        Print("No Money, close cBot");
                        Stop();

                    }

                }
                else
                {

                    ConsecutiveLoss = 0;

                }

            }
            else
            {

                ConsecutiveLoss = 0;

            }

        }

        private void _updateAverage()
        {

            double tmpAVG = 0;
            int ttBars = Bars.Count;

            for (int i = 0; i < AVGperiod; i++)
            {

                tmpAVG += Bars.HighPrices.Last(i + 1) - Bars.LowPrices.Last(i + 1);

            }

            AVG_Current = Math.Round(tmpAVG / AVGperiod, Symbol.Digits);

        }

        private void _updateInformation()
        {

            AVG_Candle = Math.Round(Math.Abs(Bars.OpenPrices.Last(0) - Bars.ClosePrices.Last(0)), Symbol.Digits);

            IsAllowed = Bars.OpenPrices.LastValue == Bars.HighPrices.LastValue || Bars.OpenPrices.LastValue == Bars.LowPrices.LastValue;
            IsInTrigger = IsAllowed && Symbol.DigitsToPips(AVG_Current) >= AVGminimum && AVG_Candle >= AVG_Current;

            SL = IsInTrigger ? Symbol.DigitsToPips(AVG_Current) : 0;
            TP = IsInTrigger ? Symbol.DigitsToPips(AVG_Current) : 0;

            // --> Creo la tabella informazioni
            if (CanDraw)
            {

                // --> Stampo a video le informazioni
                string info = string.Format( "\t{0} ({1})", NAME, VERSION );
                info += string.Format("\r\n\r\n\tAverage ({0} periods)\t{1} pips", AVGperiod, Symbol.DigitsToPips(AVG_Current).ToString("N2"));
                info += string.Format("\r\n\tCurrent\t\t\t{0} pips", Symbol.DigitsToPips(AVG_Candle).ToString("N2"));
                info += string.Format("\r\n\tMinimum\t\t{0} pips", AVGminimum.ToString("N2"));
                info += string.Format("\r\n\r\n\tIs Allowed?\t\t{0}", (IsAllowed) ? "Yes" : "No");
                info += string.Format("\r\n\tTriggering?\t\t{0}", (IsInTrigger) ? "Yes" : "No");
                info += string.Format("\r\n\r\n\tTrading?\t\t\t{0}", (TradingEnabled) ? "On" : "Off");
                info += string.Format("\r\n\tRecovery?\t\t{0}", (RecoveryEnabled) ? "On" : "Off");
                info += string.Format("\r\n\tAlerts?\t\t\t{0}", (AlertsEnabled) ? "On" : "Off");

                ChartText ThisLabel = Chart.DrawText(NAME, info, Bars.OpenTimes.LastValue, Bars.OpenPrices.LastValue, Color.FromName(TextColor.ToString()));
                ThisLabel.VerticalAlignment = VerticalAlignment.Center;
                ThisLabel.IsInteractive = false;
                ThisLabel.FontSize = TextSize;

            }

            if (!IsAlertSend && IsInTrigger)
            {

                string mex = string.Format("{0} : Price Action triggered!", SymbolName);
                IsAlertSend = true;
                Print(mex);

                if (RunningMode == RunningMode.RealTime && AlertsEnabled)
                {

                    new Thread(new ThreadStart(delegate { MessageBox.Show(mex, NAME, MessageBoxButtons.OK, MessageBoxIcon.Information); })).Start();

                }

            }

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Operazioni da compiere ogni volta che apro una posizione con questa label
        /// </summary>
        private void _onOpenPositions(PositionOpenedEventArgs eventArgs)
        {

            if (eventArgs.Position.SymbolName == Monitor1.Symbol.Name && eventArgs.Position.Label == Monitor1.Label)
            {

                Monitor1.OpenedInThisBar = true;
                Monitor1.OpenedInThisTrigger = true;

            }

        }

        private void _loop(Extensions.Monitor monitor, Extensions.MonenyManagement moneymanagement, Extensions.Monitor.BreakEvenData breakevendata, Extensions.Monitor.TrailingData trailingdata)
        {

            // --> Controllo se ho il consenso a procedere con i trigger
            _checkResetTrigger(monitor);

            // --> Condizione condivisa, filtri generali, segnano il perimetro di azione limitando l'ingresso
            bool sharedCondition = (TradingEnabled && _canCowork(monitor) && !monitor.OpenedInThisBar && !monitor.OpenedInThisTrigger && !monitor.InGAP(GAP) && !monitor.InPause(Server.Time) && monitor.Symbol.RealSpread() <= SpreadToTrigger && monitor.Positions.Length < MaxTrades);

            // --> Controllo la presenza di trigger d'ingresso tenendo conto i filtri
            bool triggerBuy = _calculateLongTrigger(_calculateLongFilter(sharedCondition));
            bool triggerSell = _calculateShortTrigger(_calculateShortFilter(sharedCondition));

            // --> Se ho entrambi i trigger qualcosa non va, lo segnalo nei log e fermo la routin
            if (triggerBuy && triggerSell)
            {

                Print("{0} {1} ERROR : trigger buy and sell !", monitor.Label, monitor.Symbol.Name);
                return;

            }

            // --> Calcolo la size in base al money management stabilito, ma prima devo resettare la misura del calcolo
            moneymanagement.PipToCalc = SL;
            double lotSize = (monitor.Info.TotalNetProfit < 0 && RecoveryMultiplier > 1 && monitor.Info.MaxVolumeInUnits > 0) ? Math.Round(monitor.Symbol.VolumeInUnitsToQuantity(monitor.Info.MaxVolumeInUnits) * RecoveryMultiplier, 2) : moneymanagement.GetLotSize();

            double volumeInUnits = monitor.Symbol.QuantityToVolumeInUnits(lotSize);

            double tmpSL = SL;
            double tmpTP = TP;

            // --> Se ho il segnale d'ingresso considerando i filtri allora procedo con l'ordine a mercato
            if (MyOpenTradeType != Extensions.OpenTradeType.Sell && triggerBuy)
            {

                // --> Devo dimensionare lo stop
                if (MyStopType == StopMode.Auto)
                {

                    double lowest = monitor.Bars.LowPrices.Minimum(AutoStopPeriod);
                    tmpSL = monitor.Symbol.DigitsToPips(monitor.Symbol.Ask - lowest);
                    tmpSL += KPips;

                    if (tmpSL < AutoMinPips)
                        tmpSL = AutoMinPips;
                    tmpTP = Math.Round(tmpSL * AutoStopRR, 2);

                    moneymanagement.PipToCalc = tmpSL;
                    lotSize = (monitor.Info.TotalNetProfit < 0 && RecoveryMultiplier > 1 && monitor.Info.MaxVolumeInUnits > 0) ? Math.Round(monitor.Symbol.VolumeInUnitsToQuantity(monitor.Info.MaxVolumeInUnits) * RecoveryMultiplier, 2) : moneymanagement.GetLotSize();

                    volumeInUnits = monitor.Symbol.QuantityToVolumeInUnits(lotSize);

                }

                ExecuteMarketRangeOrder(TradeType.Buy, monitor.Symbol.Name, volumeInUnits, SLIPPAGE, monitor.Symbol.Ask, monitor.Label, tmpSL, tmpTP);

            }
            else if (MyOpenTradeType != Extensions.OpenTradeType.Buy && triggerSell)
            {

                // --> Devo dimensionare lo stop
                if (MyStopType == StopMode.Auto)
                {

                    double highest = monitor.Bars.HighPrices.Maximum(AutoStopPeriod);
                    tmpSL = monitor.Symbol.DigitsToPips(highest - monitor.Symbol.Bid);
                    tmpSL += KPips;

                    if (tmpSL < AutoMinPips)
                        tmpSL = AutoMinPips;
                    tmpTP = Math.Round(tmpSL * AutoStopRR, 2);

                    moneymanagement.PipToCalc = tmpSL;
                    lotSize = (monitor.Info.TotalNetProfit < 0 && RecoveryMultiplier > 1 && monitor.Info.MaxVolumeInUnits > 0) ? Math.Round(monitor.Symbol.VolumeInUnitsToQuantity(monitor.Info.MaxVolumeInUnits) * RecoveryMultiplier, 2) : moneymanagement.GetLotSize();

                    volumeInUnits = monitor.Symbol.QuantityToVolumeInUnits(lotSize);

                }

                ExecuteMarketRangeOrder(TradeType.Sell, monitor.Symbol.Name, volumeInUnits, SLIPPAGE, monitor.Symbol.Bid, monitor.Label, tmpSL, tmpTP);

            }

        }

        #endregion

        #region Strategy

        /// <summary>
        /// Controlla la logica del trigger e ne resetta lo stato
        /// </summary>
        /// <param name="monitor"></param>
        private void _checkResetTrigger(Extensions.Monitor monitor)
        {

                        /*

    Bisogna sfruttare questo reset per impedire di aprire posizioni inutilmente, immaginate
    un ingresso quando il trend è fortemente direzionale, in tal caso se fossimo controtrend
    sarebbe un disastro quindi con questo flag si aspetta che il trend sia finito per tentare
    di accedere contro trend di nuovo.

 */
monitor.OpenedInThisTrigger = false;

        }

        /// <summary>
        /// Controlla e stabilisce se si devono chiudere tutte le posizioni
        /// </summary>
        private bool _checkClosePositions(Extensions.Monitor monitor)
        {

            // --> Criteri da stabilire con la strategia, monitor.Positions......
            bool numtargets = monitor.Positions.Length >= MoneyTargetTrades;

            double realmoneytarget = Math.Round((Account.Balance / 100) * MoneyTargetPercentage, monitor.Symbol.Digits);

            return (numtargets && realmoneytarget > 0 && monitor.Info.TotalNetProfit >= realmoneytarget);

        }

        /// <summary>
        /// Conferma se i criteri di filtraggio long sono stati soddisfatti
        /// </summary>
        /// <param name="condition">Filtro globale, condizione condivisa</param>
        /// <returns>I filtri long sono stati soddisfatti</returns>
        private bool _calculateLongFilter(bool condition = true)
        {

            // --> La condizione primaria deve essere presente altrimenti non serve continuare
            if (!condition)
                return false;

            // --> In caso di multi-operations non posso andare in hedging, a patto che non venga scelto esplicitamente
            if (!HedgingOpportunity && Monitor1.Info.SellPositions > 0)
                return false;

            // --> Meglio fissare la logica per l'accesso ai dati, lo stato della candela in essere
            int index = MyLoopType == LoopType.OnBar ? 1 : 0;

            // --> Criteri da stabilire
            return true;

        }

        /// <summary>
        /// Conferma se i criteri di filtraggio short sono stati soddisfatti
        /// </summary>
        /// <param name="condition">Filtro globale, condizione condivisa</param>
        /// <returns>I filtri short sono stati soddisfatti</returns>
        private bool _calculateShortFilter(bool condition = true)
        {

            // --> La condizione primaria deve essere presente altrimenti non serve continuare
            if (!condition)
                return false;

            // --> In caso di multi-operations non posso andare in hedging, a patto che non venga scelto esplicitamente
            if (!HedgingOpportunity && Monitor1.Info.BuyPositions > 0)
                return false;

            // --> Meglio fissare la logica per l'accesso ai dati, lo stato della candela in essere
            int index = MyLoopType == LoopType.OnBar ? 1 : 0;

            // --> Criteri da stabilire
            return IsInTrigger && Bars.LastBar.IsBearish();

        }

        /// <summary>
        /// Conferma se i criteri d'ingresso long sono stati soddisfatti
        /// </summary>
        /// <param name="filter">Filtro long, condizione condivisa</param>
        /// <returns>É presente una condizione di apertura long</returns>
        private bool _calculateLongTrigger(bool filter = true)
        {

            // --> Il filtro primario deve essere presente altrimenti non serve continuare
            if (!filter)
                return false;

            // --> Meglio fissare la logica per l'accesso ai dati, lo stato della candela in essere
            int index = MyLoopType == LoopType.OnBar ? 1 : 0;

            // --> Criteri da stabilire
            return IsInTrigger && Bars.LastBar.IsBullish();

        }

        /// <summary>
        /// Conferma se i criteri d'ingresso short sono stati soddisfatti
        /// </summary>
        /// <param name="filter">Filtro short, condizione condivisa</param>
        /// <returns>É presente una condizione di apertura short</returns>
        private bool _calculateShortTrigger(bool filter = true)
        {

            // --> Il filtro primario deve essere presente altrimenti non serve continuare
            if (!filter)
                return false;

            // --> Meglio fissare la logica per l'accesso ai dati, lo stato della candela in essere
            int index = MyLoopType == LoopType.OnBar ? 1 : 0;

            // --> Criteri da stabilire
            return false;

        }

        /// <summary>
        /// Restituisce le coppie attualmente a lavoro
        /// </summary>
        /// <returns></returns>
        private List<string> _getOtherCross()
        {

            List<string> OtherCross = new List<string>();

            foreach (Position position in Positions)
            {

                if (position.SymbolName != SymbolName && !OtherCross.Contains(position.SymbolName))
                    OtherCross.Add(position.SymbolName);

            }

            return OtherCross;

        }

        /// <summary>
        /// Flag che ci autorizza a lavorare nel contesto, se ho già posizioni apro
        /// </summary>
        /// <returns></returns>
        private bool _canCowork(Extensions.Monitor monitor)
        {

            return (MaxCross == 0 || monitor.Positions.Length > 0) ? true : _getOtherCross().Count < MaxCross;

        }

        private bool _checkDrawdownMode(Extensions.Monitor monitor)
        {

            bool managed = false;

            if (ddMode == DrawDownMode.Disabled)
                return managed;

            // --> controllo se ho superato il limite per il DD
            double mmmDD = _currentDDMoney();

            if (monitor.Info.TotalNetProfit > mmmDD)
                return managed;

            // --> devo agire
            switch (ddMode)
            {

                case DrawDownMode.Close:

                    monitor.CloseAllPositions();

                    Print("{0} : Closed {1} all positions, hit max drawdown {2}", MyLabel, Symbol.Name, mmmDD);
                    managed = true;

                    break;

                case DrawDownMode.Hedging:

                    if (monitor.Info.TotalLotsBuy < monitor.Info.TotalLotsSell)
                    {

                        var volumeInUnits = Symbol.QuantityToVolumeInUnits(monitor.Info.TotalLotsSell - monitor.Info.TotalLotsBuy);

                        ExecuteMarketRangeOrder(TradeType.Buy, Symbol.Name, volumeInUnits, SLIPPAGE, Symbol.Ask, MyLabel, 0, 0);
                        monitor.OpenedInThisBar = true;
                        managed = true;

                        Print("{0} : Hedged {1} all positions, hit max drawdown {2}", MyLabel, Symbol.Name, mmmDD);

                    }
                    else if (monitor.Info.TotalLotsBuy > monitor.Info.TotalLotsSell)
                    {

                        var volumeInUnits = Symbol.QuantityToVolumeInUnits(monitor.Info.TotalLotsBuy - monitor.Info.TotalLotsSell);

                        ExecuteMarketRangeOrder(TradeType.Sell, Symbol.Name, volumeInUnits, SLIPPAGE, Symbol.Bid, MyLabel, 0, 0);
                        monitor.OpenedInThisBar = true;
                        managed = true;

                        Print("{0} : Hedged {1} all positions, hit max drawdown {2}", MyLabel, Symbol.Name, mmmDD);

                    }

                    break;

            }

            return managed;

        }

        private double _currentDDMoney()
        {

            double myCapital = Account.Balance;

            switch (accCapital)
            {

                case AccCapital.FreeMargin:

                    myCapital = Account.FreeMargin;
                    break;

                case AccCapital.Equity:

                    myCapital = Account.Equity;
                    break;

            }

            // --> Restituisco la percentuale negativa
            return (Math.Round((myCapital / 100) * ddPercentage, 2)) * -1;

        }

        private void _test(TradeType trigger, Extensions.Monitor monitor, Extensions.MonenyManagement moneymanagement, string label = "TEST")
        {

            if (!_canCowork(monitor))
            {

                _log("Can't Coworing!");
                return;

            }

            moneymanagement.PipToCalc = SL;
            double volumeInUnits = monitor.Symbol.QuantityToVolumeInUnits(moneymanagement.GetLotSize());

            double tmpSL = SL;
            double tmpTP = TP;

            switch (trigger)
            {

                case TradeType.Buy:

                    // --> Devo dimensionare lo stop
                    if (MyStopType == StopMode.Auto)
                    {

                        double lowest = monitor.Bars.LowPrices.Minimum(AutoStopPeriod);
                        tmpSL = monitor.Symbol.DigitsToPips(monitor.Symbol.Ask - lowest);
                        tmpSL += KPips;

                        if (tmpSL < AutoMinPips)
                            tmpSL = AutoMinPips;
                        tmpTP = Math.Round(tmpSL * AutoStopRR, 2);

                        moneymanagement.PipToCalc = tmpSL;
                        volumeInUnits = monitor.Symbol.QuantityToVolumeInUnits(moneymanagement.GetLotSize());

                    }

                    ExecuteMarketRangeOrder(TradeType.Buy, moneymanagement.Symbol.Name, volumeInUnits, SLIPPAGE, moneymanagement.Symbol.Ask, label, tmpSL, tmpTP);
                    break;

                case TradeType.Sell:

                    // --> Devo dimensionare lo stop
                    if (MyStopType == StopMode.Auto)
                    {

                        double highest = monitor.Bars.HighPrices.Maximum(AutoStopPeriod);
                        tmpSL = monitor.Symbol.DigitsToPips(highest - monitor.Symbol.Bid);
                        tmpSL += KPips;

                        if (tmpSL < AutoMinPips)
                            tmpSL = AutoMinPips;
                        tmpTP = Math.Round(tmpSL * AutoStopRR, 2);

                        moneymanagement.PipToCalc = tmpSL;
                        volumeInUnits = monitor.Symbol.QuantityToVolumeInUnits(moneymanagement.GetLotSize());

                    }

                    ExecuteMarketRangeOrder(TradeType.Sell, moneymanagement.Symbol.Name, volumeInUnits, SLIPPAGE, moneymanagement.Symbol.Bid, label, tmpSL, tmpTP);
                    break;

            }

        }

        private void _log(string text)
        {

            if (!DebugVerbose || text.Trim().Length == 0)
                return;

            Print("{0} : {1}", NAME, text.Trim());

        }
        #endregion

        #region LICENZA & UPDATE : PRIVATE METHOD

        // --> Versione modifica, originale in ScalFibo
        private void _checkProductUpdate()
        {

            // --> Controllo solo se sono in realtime, evito le chiamate in backtest
            if (RunningMode != RunningMode.RealTime)
                return;

            // --> Organizzo i dati per la richiesta degli aggiornamenti
            Guru.API.RequestProductInfo Request = new Guru.API.RequestProductInfo
            {

                MyProduct = new Guru.Product
                {

                    ID = ID,
                    Name = NAME,
                    Version = VERSION

                },
                AccountBroker = Account.BrokerName,
                AccountNumber = Account.Number

            };

            // --> Effettuo la richiesta
            Guru.API Response = new Guru.API(Request);

            // --> Controllo per prima cosa la presenza di errori di comunicazioni
            if (Response.ProductInfo.Exception != "")
            {

                Print("{0} Exception : {1}", NAME, Response.ProductInfo.Exception);

            }
            // --> Chiedo conferma della presenza di nuovi aggiornamenti
            else if (Response.HaveNewUpdate())
            {

                string updatemex = string.Format("{0} : Updates available {1} ( {2} )", NAME, Response.ProductInfo.LastProduct.Version, Response.ProductInfo.LastProduct.Updated);

                // --> Informo l'utente con un messaggio sul grafico e nei log del cbot
                Chart.DrawStaticText(NAME + "Updates", updatemex, VerticalAlignment.Top, cAlgo.API.HorizontalAlignment.Left, Extensions.ColorFromEnum(TextColor));
                Print(updatemex);

            }

        }

        private void _checkLicense(bool bypassread = false)
        {

            if (RunningMode != RunningMode.RealTime)
                return;

            try
            {

                // --> Controllo la licenza solo dal file
                if (!bypassread)
                    licenzaInfo = licenza.GetLicenzaFromFile();

                // --> Se non ho il login chiedo di generarlo
                if (!licenzaInfo.Login)
                {

                    _createButtonLogin();
                    exitoncalculate = true;
                    return;

                }
                else
                {

                    if (licenzaInfo.Product.CompareTo(productName.ToUpper()) != 0)
                    {

                        if (MessageBox.Show("Not for this product, remove cookie session?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                            _removeCookieAndLicense(licenza);

                        exitoncalculate = true;
                        return;

                    }
                    else
                    {

                        if ((licenzaInfo.AccountBroker.CompareTo("*") != 0 && licenzaInfo.AccountBroker.CompareTo(Account.BrokerName) != 0) || (licenzaInfo.AccountNumber.CompareTo("*") != 0 && licenzaInfo.AccountNumber.CompareTo(Account.Number.ToString()) != 0))
                        {

                            if (MessageBox.Show("Not for this account, remove cookie session?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                                _removeCookieAndLicense(licenza);

                            exitoncalculate = true;
                            return;

                        }
                        else
                        {

                            if (licenzaInfo.Expire == null || licenzaInfo.Expire.Length < 1)
                            {

                                if (MessageBox.Show("Expired, remove cookie session?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                                    _removeCookieAndLicense(licenza);

                                exitoncalculate = true;
                                return;


                            }
                            else if (licenzaInfo.Expire.CompareTo("*") != 0)
                            {

                                try
                                {

                                    String[] substringsExpire = licenzaInfo.Expire.Split(',');

                                    licenzaExpire = new DateTime(Int32.Parse(substringsExpire[0].Trim()), Int32.Parse(substringsExpire[1].Trim()), Int32.Parse(substringsExpire[2].Trim()), Int32.Parse(substringsExpire[3].Trim()), Int32.Parse(substringsExpire[4].Trim()), Int32.Parse(substringsExpire[5].Trim()));


                                    if (DateTime.Compare(licenzaExpire, Server.Time) > 0)
                                    {

                                        Print("Expire : " + licenzaExpire.ToString() + " (server : " + Server.Time.ToString() + ")");
                                        exitoncalculate = false;

                                    }
                                    else
                                    {

                                        if (MessageBox.Show("Expired (" + licenzaExpire + "), remove cookie session?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                                            _removeCookieAndLicense(licenza);

                                        exitoncalculate = true;
                                        return;

                                    }

                                }
                                catch
                                {

                                    if (MessageBox.Show("Expired, remove cookie session?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                                        _removeCookieAndLicense(licenza);

                                    exitoncalculate = true;
                                    return;

                                }

                            }
                            else
                            {

                                Print("Lifetime");
                                exitoncalculate = false;

                            }

                        }

                    }

                }

            }
            catch (Exception exp)
            {

                MessageBox.Show("Encryption issue, contact support@ctrader.guru", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                licenza.RemoveLicense();
                exitoncalculate = true;

                Print("Debug : " + exp.Message);

                return;

            }

        }

        private void _createButtonLogin()
        {

            if (RunningMode != RunningMode.RealTime)
                return;

            if (DrawingDialog != null)
            {

                DrawingDialog.IsVisible = true;
                return;

            }

            StackPanel stackPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = API.HorizontalAlignment.Center,
                Orientation = API.Orientation.Vertical,
                IsVisible = false,
                Width = 200,
                BackgroundColor = Color.Red,
                Margin = new Thickness(10, 10, 10, 10)
            };

            Button btnLogin = new Button
            {
                Text = "CTRADER GURU - LOGIN",
                BackgroundColor = Color.Red,
                ForegroundColor = Color.White,
                Top = 10,
                CornerRadius = 0,
                HorizontalContentAlignment = API.HorizontalAlignment.Center

            };
            btnLogin.Click += delegate
            {

                if (!exitoncalculate)
                    return;

                DrawingDialog.IsVisible = false;
                System.Windows.Forms.Application.DoEvents();

                try
                {


                    _createLicense();

                    OnStart();
                    OnTick();

                    DrawingDialog.IsVisible = false;
                    System.Windows.Forms.Application.DoEvents();

                }
                catch
                {
                }


            };

            stackPanel.AddChild(btnLogin);

            DrawingDialog = stackPanel;
            Chart.AddControl(DrawingDialog);

            DrawingDialog.IsVisible = true;

        }

        private void _createLicense()
        {

            if (RunningMode != RunningMode.RealTime)
                return;

            // --> Chiedo al server con i cookie, ma prima tento il recupero dal file
            licenzaInfo = licenza.GetLicenzaFromFile();

            if (licenzaInfo.ErrorProc == -2000)
            {

                MessageBox.Show("Waiting");
                return;
            }

            if (!licenzaInfo.Login || licenzaInfo.ErrorProc != 1000)
                licenzaInfo = licenza.GetLicenzaFromServer();

            // --> Ci sono problemi con i cookie
            if (licenzaInfo.ErrorProc == 2 || licenzaInfo.ErrorProc == 3 || licenzaInfo.Login == false)
            {
                // --> Rimuovo i cookie comunque
                licenza.RemoveCookie();
                licenza.RemoveLicense();


                // --> Li rigenero chiedendo il login, faccio attenzione ad altri processi
                Process[] processlist = Process.GetProcesses();
                bool finded = false;

                foreach (Process process in processlist)
                {

                    if (!String.IsNullOrEmpty(process.MainWindowTitle) && process.MainWindowTitle.ToUpper().CompareTo("CTRADER GURU - LOGIN") == 0)
                    {

                        finded = true;
                        break;

                    }

                }

                if (!finded)
                {


                    frmLogin LoginForm = new frmLogin(Account.BrokerName, Account.Number.ToString());
                    LoginForm.FormClosed += delegate
                    {

                        licenzaInfo = licenza.GetLicenzaFromServer();
                        _checkLicense();

                    };

                    LoginForm.ShowDialog();

                }
                else
                {

                    _alertChart("Others are logging in, waiting...");
                }

                exitoncalculate = true;

            }
            else
            {

                _checkLicense(true);

            }

        }

        private void _removeCookieAndLicense(CL_CTG_Licenza licenza)
        {

            licenza.RemoveCookie();
            licenza.RemoveLicense();

        }

        private void _alertChart(string mymex, bool withPrint = true)
        {

            if (RunningMode != RunningMode.RealTime)
                return;

            string mex = string.Format("{0} : {1}", NAME.ToUpper(), mymex);

            Chart.DrawStaticText("alert", mex, VerticalAlignment.Center, API.HorizontalAlignment.Center, Color.Red);
            if (withPrint)
                Print(mex);

        }

        #endregion

    }

}

/// <summary>
/// NameSpace che racchiude tutte le feature ctrader.guru
/// </summary>
namespace Guru
{
    /// <summary>
    /// Classe che definisce lo standard identificativo del prodotto nel marketplace ctrader.guru
    /// </summary>
    public class Product
    {

        public int ID = 0;
        public string Name = "";
        public string Version = "";
        public string Updated = "";
        public string LastCheck = "";

    }

    /// <summary>
    /// Offre la possibilità di utilizzare le API messe a disposizione da ctrader.guru per verificare gli aggiornamenti del prodotto.
    /// Permessi utente "AccessRights = AccessRights.FullAccess" per accedere a internet ed utilizzare JSON
    /// </summary>
    public class API
    {
        /// <summary>
        /// Costante da non modificare, corrisponde alla pagina dei servizi API
        /// </summary>
        private const string Service = "https://ctrader.guru/api/product_info/";

        private static string MainPath = string.Format("{0}\\cAlgo\\cTraderGuru\\", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        private readonly string InfoFile = string.Format("{0}update", MainPath);

        /// <summary>
        /// Costante da non modificare, utilizzata per filtrare le richieste
        /// </summary>
        private const string UserAgent = "cTrader Guru";

        /// <summary>
        /// Variabile dove verranno inserite le direttive per la richiesta
        /// </summary>
        private RequestProductInfo RequestProduct = new RequestProductInfo();

        /// <summary>
        /// Variabile dove verranno inserite le informazioni identificative dal server dopo l'inizializzazione della classe API
        /// </summary>
        public ResponseProductInfo ProductInfo = new ResponseProductInfo();

        /// <summary>
        /// Classe che formalizza i parametri di richiesta, vengono inviate le informazioni del prodotto e di profilazione a fini statistici
        /// </summary>
        public class RequestProductInfo
        {

            /// <summary>
            /// Il prodotto corrente per il quale richiediamo le informazioni
            /// </summary>
            public Product MyProduct = new Product();

            /// <summary>
            /// Broker con il quale effettiamo la richiesta
            /// </summary>
            public string AccountBroker = "";

            /// <summary>
            /// Il numero di conto con il quale chiediamo le informazioni
            /// </summary>
            public int AccountNumber = 0;

        }

        /// <summary>
        /// Classe che formalizza lo standard per identificare le informazioni del prodotto
        /// </summary>
        public class ResponseProductInfo
        {

            /// <summary>
            /// Il prodotto corrente per il quale vengono fornite le informazioni
            /// </summary>
            public Product LastProduct = new Product();

            /// <summary>
            /// Eccezioni in fase di richiesta al server, da utilizzare per controllare l'esito della comunicazione
            /// </summary>
            public string Exception = "";

            /// <summary>
            /// La risposta del server
            /// </summary>
            public string Source = "";

        }

        /// <summary>
        /// Richiede le informazioni del prodotto richiesto
        /// </summary>
        /// <param name="Request"></param>
        public API(RequestProductInfo Request)
        {

            RequestProduct = Request;

            // --> Non controllo se non ho l'ID del prodotto
            if (Request.MyProduct.ID <= 0)
                return;

            string cleanedproduct = string.Join("-", Request.MyProduct.Name.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            string fileToCheck = InfoFile + "-" + cleanedproduct.ToUpper() + ".json";

            // --> Controllo che siano passati almeno 30minuti tra una richiesta e l'altra
            try
            {

                string infodata = File.ReadAllText(fileToCheck);

                Product infolocal = JsonConvert.DeserializeObject<Product>(infodata);

                if (infolocal.LastCheck != "" && infolocal.ID == Request.MyProduct.ID)
                {

                    DateTime timeToTrigger = DateTime.Parse(infolocal.LastCheck).AddMinutes(60);

                    // --> Controllo se ci sono le condizioni per procedere
                    if (DateTime.Compare(timeToTrigger, DateTime.Now) > 0)
                    {

                        ProductInfo.LastProduct = infolocal;
                        return;

                    }

                }

            }
            catch
            {

            }

            // --> Dobbiamo supervisionare la chiamata per registrare l'eccexione
            try
            {

                // --> Strutturo le informazioni per la richiesta POST
                NameValueCollection data = new NameValueCollection
                {
                    {
                        "account_broker",
                        Request.AccountBroker
                    },
                    {
                        "account_number",
                        Request.AccountNumber.ToString()
                    },
                    {
                        "my_version",
                        Request.MyProduct.Version
                    },
                    {
                        "productid",
                        Request.MyProduct.ID.ToString()
                    }
                };

                // --> Autorizzo tutte le pagine di questo dominio
                Uri myuri = new Uri(Service);
                string pattern = string.Format("{0}://{1}/.*", myuri.Scheme, myuri.Host);

                Regex urlRegEx = new Regex(pattern);
                WebPermission p = new WebPermission(NetworkAccess.Connect, urlRegEx);
                p.Assert();

                // --> Protocollo di sicurezza https://
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;

                // -->> Richiedo le informazioni al server
                using (var wb = new WebClient())
                {

                    wb.Headers.Add("User-Agent", UserAgent);

                    var response = wb.UploadValues(myuri, "POST", data);
                    ProductInfo.Source = Encoding.UTF8.GetString(response);

                }

                // -->>> Nel cBot necessita l'attivazione di "AccessRights = AccessRights.FullAccess"
                ProductInfo.LastProduct = JsonConvert.DeserializeObject<Product>(ProductInfo.Source);
                ProductInfo.LastProduct.LastCheck = DateTime.Now.ToString();

                // --> Aggiorno il file locale
                try
                {

                    Directory.CreateDirectory(MainPath);

                    File.WriteAllText(fileToCheck, JsonConvert.SerializeObject(ProductInfo.LastProduct));

                }
                catch
                {
                }

            }
            catch (Exception Exp)
            {

                // --> Qualcosa è andato storto, registro l'eccezione
                ProductInfo.Exception = Exp.Message;

            }

        }

        /// <summary>
        /// Esegue un confronto tra le versioni per determinare la presenza di aggiornamenti
        /// </summary>
        /// <returns></returns>
        public bool HaveNewUpdate()
        {

            // --> Voglio essere sicuro che stiamo lavorando con le informazioni giuste
            return (ProductInfo.LastProduct.ID == RequestProduct.MyProduct.ID && ProductInfo.LastProduct.Version != "" && RequestProduct.MyProduct.Version != "" && ProductInfo.LastProduct.Version != null && new Version(RequestProduct.MyProduct.Version).CompareTo(new Version(ProductInfo.LastProduct.Version)) < 0);

        }

    }

}
