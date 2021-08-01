using System;
using System.Collections.Generic;
using SetYourTone.Models;
using System.Text;

namespace SetYourTone.Services
{
    public class SpaceLimiter : Ruler
    {
        protected int XLeft;
        protected int YTop;
        protected int XRight;
        protected int YFoot;
        public SpaceLimiter (RuleModel RuleParameters, Dictionary<string, char> inputTriggers, string currentUserFrame)
            : base(RuleParameters, inputTriggers)
        {
            string[] splittedFrame = currentUserFrame.Split(';');
            XLeft = Convert.ToInt32(splittedFrame[0]);
            YTop = Convert.ToInt32(splittedFrame[1]);
            XRight = Convert.ToInt32(splittedFrame[2]);
            YFoot = Convert.ToInt32(splittedFrame[3]);
            deep = YFoot + 1;

            _RuleParameters = RuleParameters;
            _Triggers = inputTriggers;
            topLayer = _RuleParameters.StartLayerWorkPiece;
            finalTopLayerCenter = CenterFinder(topLayer.Length);
            BorderEditor();
            finalTopLayerCenter = CenterFinder(_RuleParameters.LeftBorder.Length + topLayer.Length + _RuleParameters.RightBorder.Length);
            frame = Framer(XLeft, YTop, XRight, YFoot);
        }
        protected override void BorderEditor()
        {
            //Если пользователь не дал строк они = "".
            if (_RuleParameters.LeftBorder == null) _RuleParameters.LeftBorder = "";
            if (_RuleParameters.RightBorder == null) _RuleParameters.RightBorder = "";
            //Если они в итоге меньше расстояния, затрагиваемого триггером, то они дополняются с внешней стороны строкой Default символов.
            if (_RuleParameters.LeftBorder.Length < maxStepAnalyzer('r'))
            {
                _RuleParameters.LeftBorder = DefaultSymbolLine(maxStepAnalyzer('r') - _RuleParameters.LeftBorder.Length) + _RuleParameters.LeftBorder;
            }
            if (_RuleParameters.RightBorder.Length < maxStepAnalyzer('l'))
            {
                _RuleParameters.RightBorder = _RuleParameters.RightBorder + DefaultSymbolLine(maxStepAnalyzer('l') - _RuleParameters.RightBorder.Length);
            }
            //В режиме ограниченного пространства, границы расширяются под координаты, если кадр выходит за текущие границы.
            //finalTopLayerCenter на текущий момент является TopLayerCenter, центром строки от пользователя без границ, не расширенной.
            if ((XLeft < 0)&&(Math.Abs(XLeft)>finalTopLayerCenter + _RuleParameters.LeftBorder.Length))
            {
                string leftBorderStencil = DefaultSymbolLine(Math.Abs(XLeft) - (finalTopLayerCenter + _RuleParameters.LeftBorder.Length));
                _RuleParameters.LeftBorder = leftBorderStencil + _RuleParameters.LeftBorder;
            }
            if ((XRight > 0) && (Math.Abs(XRight) > topLayer.Length - finalTopLayerCenter - 1 + _RuleParameters.RightBorder.Length))
            {
                string rightBorderStencil = DefaultSymbolLine(Math.Abs(XRight) - (topLayer.Length - finalTopLayerCenter - 1 + _RuleParameters.RightBorder.Length));
                _RuleParameters.RightBorder = _RuleParameters.RightBorder + rightBorderStencil;
            }
        }

    }
}
