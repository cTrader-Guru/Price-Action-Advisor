/*  CTRADER GURU

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


namespace cAlgo
{

    public static class Extensions
    {

        #region Enum

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

        public enum CapitalTo
        {

            Balance,
            Equity

        }

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

        public class Monitor
        {

            private readonly Positions _allPositions = null;

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

            public class PauseTimes
            {

                public double Over = 0;
                public double Under = 0;

            }

            public class BreakEvenData
            {

                public bool OnlyFirst = false;
                public ProfitDirection ProfitDirection = ProfitDirection.All;
                public double Activation = 0;
                public int LimitBar = 0;
                public double Distance = 0;

            }

            public class TrailingData
            {

                public bool OnlyFirst = false;
                public bool ProActive = false;
                public double Activation = 0;
                public double Distance = 0;

            }

            public bool OpenedInThisBar = false;

            public bool OpenedInThisTrigger = false;

            public readonly string Label;

            public readonly Symbol Symbol;

            public readonly Bars Bars;

            public readonly PauseTimes Pause;

            public Information Info { get; private set; }

            public Position[] Positions { get; private set; }

            public Monitor(string NewLabel, Symbol NewSymbol, Bars NewBars, Positions AllPositions, PauseTimes NewPause)
            {

                Label = NewLabel;
                Symbol = NewSymbol;
                Bars = NewBars;
                Pause = NewPause;

                Info = new Information();

                _allPositions = AllPositions;

                Update(false, null, null, 0);

            }

            public Information Update(bool closeall, BreakEvenData breakevendata, TrailingData trailingdata, double SafeLoss, TradeType? filtertype = null)
            {

                Positions = _allPositions.FindAll(Label, Symbol.Name);

                double highestHighAfterFirstOpen = (Positions.Length > 0) ? Info.HighestHighAfterFirstOpen : 0;
                double lowestLowAfterFirstOpen = (Positions.Length > 0) ? Info.LowestLowAfterFirstOpen : 0;

                Info = new Information 
                {

                    HighestHighAfterFirstOpen = highestHighAfterFirstOpen,
                    LowestLowAfterFirstOpen = lowestLowAfterFirstOpen

                };

                double tmpVolume = 0;

                foreach (Position position in Positions)
                {

                    if (Info.HighestHighAfterFirstOpen == 0 || Symbol.Ask > Info.HighestHighAfterFirstOpen)
                        Info.HighestHighAfterFirstOpen = Symbol.Ask;
                    if (Info.LowestLowAfterFirstOpen == 0 || Symbol.Bid < Info.LowestLowAfterFirstOpen)
                        Info.LowestLowAfterFirstOpen = Symbol.Bid;

                    if (closeall && (filtertype == null || position.TradeType == filtertype))
                    {

                        position.Close();
                        continue;

                    }

                    if (SafeLoss > 0 && position.StopLoss == null)
                    {

                        TradeResult result = position.ModifyStopLossPips(SafeLoss);

                        if (result.Error == ErrorCode.InvalidRequest || result.Error == ErrorCode.InvalidStopLossTakeProfit)
                        {

                            position.Close();

                        }

                        continue;

                    }

                    if (breakevendata != null && (!breakevendata.OnlyFirst || Positions.Length == 1))
                        CheckBreakEven(position, breakevendata);

                    if (trailingdata != null && (!trailingdata.OnlyFirst || Positions.Length == 1))
                        CheckTrailing(position, trailingdata);

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

            public void CloseAllPositions(TradeType? filtertype = null)
            {

                Update(true, null, null, 0, filtertype);

            }

            public bool InGAP(double distance)
            {

                return Symbol.DigitsToPips(Bars.LastGAP()) >= distance;

            }

            public bool InPause(DateTime timeserver)
            {

                string nowHour = (timeserver.Hour < 10) ? string.Format("0{0}", timeserver.Hour) : string.Format("{0}", timeserver.Hour);
                string nowMinute = (timeserver.Minute < 10) ? string.Format("0{0}", timeserver.Minute) : string.Format("{0}", timeserver.Minute);

                double adesso = Convert.ToDouble(string.Format("{0},{1}", nowHour, nowMinute));

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

            private void CheckBreakEven(Position position, BreakEvenData breakevendata)
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


            private void CheckTrailing(Position position, TrailingData trailingdata)
            {

                if (trailingdata == null || trailingdata.Activation == 0 || trailingdata.Distance == 0)
                    return;
                double distance = Symbol.PipsToDigits(trailingdata.Distance);
                double activation = Symbol.PipsToDigits(trailingdata.Activation);


                double trailing;
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

                            double activationprice = position.EntryPrice + activation;
                            double firsttrailing = Math.Round(activationprice - distance, Symbol.Digits);

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

                            double activationprice = position.EntryPrice - activation;
                            double firsttrailing = Math.Round(activationprice + distance, Symbol.Digits);

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

        public class MonenyManagement
        {

            private readonly double _minSize = 0.01;
            private double _percentage = 0;
            private double _fixedSize = 0;
            private double _pipToCalc = 30;

            private readonly IAccount _account = null;
            public readonly Symbol Symbol;

            public CapitalTo CapitalType = CapitalTo.Balance;

            public double Percentage
            {

                get { return _percentage; }


                set { _percentage = (value > 0 && value <= 100) ? value : 0; }
            }

            public double FixedSize
            {

                get { return _fixedSize; }



                set { _fixedSize = (value >= _minSize) ? value : 0; }
            }

            public double PipToCalc
            {

                get { return _pipToCalc; }

                set { _pipToCalc = (value > 0) ? value : 100; }
            }

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

            public MonenyManagement(IAccount NewAccount, CapitalTo NewCapitalTo, double NewPercentage, double NewFixedSize, double NewPipToCalc, Symbol NewSymbol)
            {

                _account = NewAccount;

                Symbol = NewSymbol;

                CapitalType = NewCapitalTo;
                Percentage = NewPercentage;
                FixedSize = NewFixedSize;
                PipToCalc = NewPipToCalc;

            }

            public double GetLotSize()
            {

                if (FixedSize > 0)
                    return FixedSize;

                double moneyrisk = Capital / 100 * Percentage;

                double sl_double = PipToCalc * Symbol.PipSize;
                double lots = Math.Round(Symbol.VolumeInUnitsToQuantity(moneyrisk / ((sl_double * Symbol.TickValue) / Symbol.TickSize)), 2);

                if (lots < _minSize)
                    return _minSize;

                return lots;

            }

        }

        #endregion

        #region Helper

        public static API.Color ColorFromEnum(ColorNameEnum colorName)
        {

            return API.Color.FromName(colorName.ToString("G"));

        }

        #endregion

        #region Bars

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

        public static double Body(this Bar thisBar)
        {

            return thisBar.IsBullish() ? thisBar.Close - thisBar.Open : thisBar.Open - thisBar.Close;


        }

        public static bool IsBullish(this Bar thisBar)
        {

            return thisBar.Close > thisBar.Open;

        }

        public static bool IsBearish(this Bar thisBar)
        {

            return thisBar.Close < thisBar.Open;

        }

        public static bool IsDoji(this Bar thisBar)
        {

            return thisBar.Close == thisBar.Open;

        }

        #endregion

        #region Symbol

        public static double DigitsToPips(this Symbol thisSymbol, double Pips)
        {

            return Math.Round(Pips / thisSymbol.PipSize, 2);

        }

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


        public const string NAME = "Price Action Advisor";

        public const string VERSION = "1.0.8";

        #endregion

        #region Params

        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://www.google.com/search?q=ctrader+guru+price+action+advisor")]
        public string ProductInfo { get; set; }

        [Parameter("Label ( Magic Name )", Group = "Identity", DefaultValue = NAME)]
        public string MyLabel { get; set; }

        [Parameter("Preset information", Group = "Identity", DefaultValue = "EURUSD 1H")]
        public string PresetInfo { get; set; }

        [Parameter("Period (bars)", Group = "Average", DefaultValue = 50, MinValue = 2)]
        public int AVGperiod { get; set; }

        [Parameter("Minimum (pips; zero = all)", Group = "Average", DefaultValue = 0, MinValue = 0, Step = 0.1)]
        public double AVGminimum { get; set; }

        [Parameter("Trading ?", Group = "Trading", DefaultValue = true)]
        public bool TradingEnabled { get; set; }

        [Parameter("SL/TP Enabled ?", Group = "Trading", DefaultValue = true)]
        public bool SLTPEnabled { get; set; }

        [Parameter("Recovery ?", Group = "Trading", DefaultValue = true)]
        public bool RecoveryEnabled { get; set; }

        [Parameter("K + SL/TP (pips)", Group = "Trading", DefaultValue = 1.5, MinValue = 0, Step = 0.1)]
        public double KSLTP { get; set; }

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
        readonly double StopLevel = 5;

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

        protected override void OnStart()
        {

            Print("{0} : {1}", NAME, VERSION);

            SafeLoss = (MyStopType == StopMode.Auto || SL > 0) ? StopLevel : 0;
            CanDraw = (RunningMode == RunningMode.RealTime || RunningMode == RunningMode.VisualBacktesting);

            UpdateAverage();

            UpdateInformation();

            Pause1 = new Extensions.Monitor.PauseTimes 
            {

                Over = PauseOver,
                Under = PauseUnder

            };

            Monitor1 = new Extensions.Monitor(MyLabel, Symbol, Bars, Positions, Pause1);

            MonenyManagement1 = new Extensions.MonenyManagement(Account, MyCapital, MyRisk, FixedLots, SL > 0 ? SL : FakeSL, Symbol);

            BreakEvenData1 = new Extensions.Monitor.BreakEvenData 
            {

                OnlyFirst = BreakEvenProtectionType == ProtectionType.OnlyFirst,
                ProfitDirection = BreakEvenProfitDirection,
                Activation = (BreakEvenProtectionType != ProtectionType.Disabled) ? BreakEvenActivation : 0,
                LimitBar = BreakEvenLimitBars,
                Distance = BreakEvenDistance

            };

            TrailingData1 = new Extensions.Monitor.TrailingData 
            {

                OnlyFirst = TrailingProtectionType == ProtectionType.OnlyFirst,
                ProActive = TrailingProactive,
                Activation = (TrailingProtectionType != ProtectionType.Disabled) ? TrailingActivation : 0,
                Distance = TrailingDistance

            };

            Positions.Opened += OnOpenPositions;
            Positions.Closed += OnPositionsClosed;

            if (OpenOnStart != MyTradeType.Disabled)
                Test((OpenOnStart == MyTradeType.Buy) ? TradeType.Buy : TradeType.Sell, Monitor1, MonenyManagement1, MyLabel);

        }

        protected override void OnStop()
        {

            Positions.Opened -= OnOpenPositions;

        }

        protected override void OnBar()
        {

            IsAlertSend = false;

            UpdateAverage();

            Monitor1.OpenedInThisBar = false;

            if (MyLoopType == LoopType.OnBar)
                Loop(Monitor1, MonenyManagement1, BreakEvenData1, TrailingData1);

        }

        protected override void OnTick()
        {

            Monitor1.Update(CheckClosePositions(Monitor1), Monitor1.Info.IAmInHedging ? null : BreakEvenData1, Monitor1.Info.IAmInHedging ? null : TrailingData1, SafeLoss, null);

            UpdateInformation();

            if (Monitor1.Info.IAmInHedging || CheckDrawdownMode(Monitor1))
                return;

            if (Boring > 0 && Monitor1.Positions.Length >= 2 && Monitor1.Info.TotalNetProfit > 0)
            {

                int currentIndex = Bars.Count - 1;
                int indexFirstTrade = Bars.OpenTimes.GetIndexByTime(Monitor1.Info.FirstPosition.EntryTime);

                if ((currentIndex - indexFirstTrade) >= Boring)
                {

                    Monitor1.CloseAllPositions();
                    Log("Closed for Boring Bars ");
                    return;
                }

            }

            if (MyLoopType == LoopType.OnTick)
                Loop(Monitor1, MonenyManagement1, BreakEvenData1, TrailingData1);

        }

        private void OnPositionsClosed(PositionClosedEventArgs args)
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

        private void UpdateAverage()
        {

            double tmpAVG = 0;

            for (int i = 0; i < AVGperiod; i++)
            {

                tmpAVG += Bars.HighPrices.Last(i + 1) - Bars.LowPrices.Last(i + 1);

            }

            AVG_Current = Math.Round(tmpAVG / AVGperiod, Symbol.Digits);

        }

        private void UpdateInformation()
        {

            AVG_Candle = Math.Round(Math.Abs(Bars.OpenPrices.Last(0) - Bars.ClosePrices.Last(0)), Symbol.Digits);

            IsAllowed = Bars.OpenPrices.LastValue == Bars.HighPrices.LastValue || Bars.OpenPrices.LastValue == Bars.LowPrices.LastValue;
            IsInTrigger = IsAllowed && Symbol.DigitsToPips(AVG_Current) >= AVGminimum && AVG_Candle >= AVG_Current;

            SL = IsInTrigger && SLTPEnabled ? Math.Round(Symbol.DigitsToPips(AVG_Current) + KSLTP, 1) : 0;
            TP = SL;

            if (CanDraw)
            {

                string info = string.Format("\t{0} ({1})", NAME, VERSION);
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

        private void OnOpenPositions(PositionOpenedEventArgs eventArgs)
        {

            if (eventArgs.Position.SymbolName == Monitor1.Symbol.Name && eventArgs.Position.Label == Monitor1.Label)
            {

                Monitor1.OpenedInThisBar = true;
                Monitor1.OpenedInThisTrigger = true;

            }

        }

        private void Loop(Extensions.Monitor monitor, Extensions.MonenyManagement moneymanagement, Extensions.Monitor.BreakEvenData breakevendata, Extensions.Monitor.TrailingData trailingdata)
        {

            CheckResetTrigger(monitor);

            bool sharedCondition = (TradingEnabled && CanCowork(monitor) && !monitor.OpenedInThisBar && !monitor.OpenedInThisTrigger && !monitor.InGAP(GAP) && !monitor.InPause(Server.Time) && monitor.Symbol.RealSpread() <= SpreadToTrigger && monitor.Positions.Length < MaxTrades);

            bool triggerBuy = CalculateLongTrigger(CalculateLongFilter(sharedCondition));
            bool triggerSell = CalculateShortTrigger(CalculateShortFilter(sharedCondition));

            if (triggerBuy && triggerSell)
            {

                Print("{0} {1} ERROR : trigger buy and sell !", monitor.Label, monitor.Symbol.Name);
                return;

            }

            moneymanagement.PipToCalc = SL;
            double lotSize = (monitor.Info.TotalNetProfit < 0 && RecoveryMultiplier > 1 && monitor.Info.MaxVolumeInUnits > 0) ? Math.Round(monitor.Symbol.VolumeInUnitsToQuantity(monitor.Info.MaxVolumeInUnits) * RecoveryMultiplier, 2) : moneymanagement.GetLotSize();

            double volumeInUnits = monitor.Symbol.QuantityToVolumeInUnits(lotSize);

            double tmpSL = SL;
            double tmpTP = TP;

            if (MyOpenTradeType != Extensions.OpenTradeType.Sell && triggerBuy)
            {

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

        private void CheckResetTrigger(Extensions.Monitor monitor)
        {

            monitor.OpenedInThisTrigger = false;

        }

        private bool CheckClosePositions(Extensions.Monitor monitor)
        {

            bool numtargets = monitor.Positions.Length >= MoneyTargetTrades;

            double realmoneytarget = Math.Round((Account.Balance / 100) * MoneyTargetPercentage, monitor.Symbol.Digits);

            return (numtargets && realmoneytarget > 0 && monitor.Info.TotalNetProfit >= realmoneytarget);

        }

        private bool CalculateLongFilter(bool condition = true)
        {

            if (!condition || (!HedgingOpportunity && Monitor1.Info.SellPositions > 0) || Bars.Last(1).IsBearish())
                return false;

            return true;

        }

        private bool CalculateShortFilter(bool condition = true)
        {

            if (!condition || (!HedgingOpportunity && Monitor1.Info.BuyPositions > 0) || Bars.Last(1).IsBullish())
                return false;

            return true;

        }

        private bool CalculateLongTrigger(bool filter = true)
        {

            if (!filter)
                return false;

            return IsInTrigger && Bars.LastBar.IsBullish();

        }

        private bool CalculateShortTrigger(bool filter = true)
        {

            if (!filter)
                return false;

            return IsInTrigger && Bars.LastBar.IsBearish();

        }

        private List<string> GetOtherCross()
        {

            List<string> OtherCross = new List<string>();

            foreach (Position position in Positions)
            {

                if (position.SymbolName != SymbolName && !OtherCross.Contains(position.SymbolName))
                    OtherCross.Add(position.SymbolName);

            }

            return OtherCross;

        }

        private bool CanCowork(Extensions.Monitor monitor)
        {

            return (MaxCross == 0 || monitor.Positions.Length > 0) || GetOtherCross().Count < MaxCross;

        }

        private bool CheckDrawdownMode(Extensions.Monitor monitor)
        {

            bool managed = false;

            if (ddMode == DrawDownMode.Disabled)
                return managed;

            double mmmDD = CurrentDDMoney();

            if (monitor.Info.TotalNetProfit > mmmDD)
                return managed;

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

        private double CurrentDDMoney()
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

            return (Math.Round((myCapital / 100) * ddPercentage, 2)) * -1;

        }

        private void Test(TradeType trigger, Extensions.Monitor monitor, Extensions.MonenyManagement moneymanagement, string label = "TEST")
        {

            if (!CanCowork(monitor))
            {

                Log("Can't Coworing!");
                return;

            }

            moneymanagement.PipToCalc = SL;
            double volumeInUnits = monitor.Symbol.QuantityToVolumeInUnits(moneymanagement.GetLotSize());

            double tmpSL = SL;
            double tmpTP = TP;

            switch (trigger)
            {

                case TradeType.Buy:

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

        private void Log(string text)
        {

            if (!DebugVerbose || text.Trim().Length == 0)
                return;

            Print("{0} : {1}", NAME, text.Trim());

        }
        #endregion

    }

}
