using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SetYourTone.Models;
using System.Text;

namespace SetYourTone.Services
{
    public class Ruler
    {
        public char[,] frame;
        protected string topLayer;
        protected int deep;
        protected Dictionary<string, char> _Triggers;
        protected int finalTopLayerCenter;
        protected RuleModel _RuleParameters;

        protected Ruler (RuleModel RuleParameters, Dictionary<string, char> inputTriggers)
        {
            _RuleParameters = RuleParameters;
            _Triggers = inputTriggers;
        }
        protected string DefaultSymbolLine(int length)
        {
            StringBuilder line = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                line.Append(_RuleParameters.DefaultCellMean);
            }
            return line.ToString();
        }
        protected int CenterFinder(int userLayerLength)
        {
            if (userLayerLength % 2 == 1)
                return userLayerLength / 2;
            else
                return userLayerLength / 2 - 1;
        }
        protected int maxStepAnalyzer(char side)
        {
            int maxOfst = 0;
            if (side == 'l')
            {
                maxOfst = _RuleParameters.Offset + _RuleParameters.Length - 1;
            }
            else
            {
                maxOfst = Math.Abs(_RuleParameters.Offset);
            }
            return maxOfst;
        }
        protected virtual void BorderEditor()
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
        }
        //Функция для получения внутренней строки на основе предыдущей и присоединения границ.
        protected void LineCalculator(in char[] inputLine, out char[] outputLine)
        {
            outputLine = new char[inputLine.Length];
            StringBuilder triggerLineSculpt = new StringBuilder(_RuleParameters.Length);
            string currentTriggerLine;
            //Вычисление внутренней части, без границ
            for (int i = _RuleParameters.LeftBorder.Length; i < inputLine.Length - _RuleParameters.RightBorder.Length; i++)
            {
                //Получаю подстроку для поиска совпадения в _Triggers
                for (int j = 0; j < _RuleParameters.Length; j++)
                {
                    //Преобразование для бинарного триггера
                    if (inputLine[i + _RuleParameters.Offset + j] != '0')
                        triggerLineSculpt.Append('1');
                    else
                        triggerLineSculpt.Append('0');
                }
                currentTriggerLine = triggerLineSculpt.ToString();
                triggerLineSculpt.Clear();
                //Поиск наличия в триггерах и занесение значения в клетку, не найден - значение по умолчанию.
                if (_Triggers.TryGetValue(currentTriggerLine, out char value))
                {
                    outputLine[i] = value;
                }
                else
                {
                    outputLine[i] = _RuleParameters.DefaultCellMean;
                }
                //Занесение границ в строку
                for (int k = 0; k < _RuleParameters.LeftBorder.Length; k++)
                {
                    outputLine[k] = _RuleParameters.LeftBorder[k];
                }
                for (int k = inputLine.GetLength(0) - _RuleParameters.RightBorder.Length; k < inputLine.GetLength(0); k++)
                {
                    outputLine[k] = _RuleParameters.LeftBorder[k - inputLine.GetLength(0) + _RuleParameters.RightBorder.Length];
                }
            }
        }
        //Досчитывает с помощью LineCalculator до начала кадра и заполняет его.
        protected char[,] Framer(int XLeft, int YTop, int XRight, int YFoot)
        {
            XLeft = XLeft + finalTopLayerCenter;
            XRight = XRight + finalTopLayerCenter;
            //Размеры кадра
            int Xlength = XRight - XLeft + 1;
            int Ylength = YFoot - YTop + 1;
            //Строки для вычисления, second на основе first по правилу.
            string finalFirstLine = _RuleParameters.LeftBorder + topLayer + _RuleParameters.RightBorder;
            char[] first = finalFirstLine.ToCharArray();
            char[] second;

            for (int i = 0; i < YTop; i++)
            {
                LineCalculator(in first, out second);
                first = second;
            }

            //Кадр, который передаётся пользователю
            char[,] frame = new char[Ylength, Xlength];
            for (int i = 0; i < Ylength; i++)
            {
                for (int j = 0; j < Xlength; j++)
                {
                    frame[i, j] = first[XLeft + j];
                }
                LineCalculator(first, out second);
                first = second;
            }
            return frame;
        }
    }
}
