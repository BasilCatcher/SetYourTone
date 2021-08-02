using System;
using System.Collections.Generic;
using SetYourTone.Models;

namespace SetYourTone.Services
{
    public class EndlessTopLayer : Ruler
    {
        protected int XLeft;
        protected int YTop;
        protected int XRight;
        protected int YFoot;
        public EndlessTopLayer(RuleModel RuleParameters, Dictionary<string, char> inputTriggers, string currentUserFrame)
            : base (RuleParameters, inputTriggers)
        {
            string[] splittedFrame = currentUserFrame.Split(';');
            XLeft = Convert.ToInt32(splittedFrame[0]);
            YTop = Convert.ToInt32(splittedFrame[1]);
            XRight = Convert.ToInt32(splittedFrame[2]);
            YFoot = Convert.ToInt32(splittedFrame[3]);
            deep = YFoot + 1;

            BorderEditor();
            topLayer = TopLayerExpander(_RuleParameters.StartLayerWorkPiece,
                                maxStepAnalyzer('l'), maxStepAnalyzer('r'),
                                XLeft, XRight,
                                _RuleParameters.LeftBorder.Length, deep, out finalTopLayerCenter);

            frame = Framer(XLeft, YTop, XRight, YFoot);
        }
        //Расширяет строку пользователя, даёт её центр для правильного расчёта координат
        private string TopLayerExpander(string workPiece, int leftMoving, int rightMoving,
                                int frameLeft, int frameRight,
                                int leftBorderLength, int howDeepWeGo, out int center)
        {
            //На каждой строке автомат может затронуть некоторое максимальное
            //количество клеток левее/правее относительно верхних, на основе широты захвата триггеров,
            //и так можно определить зоны влево и вправо, затронутые правилом через несколько рядов
            //если кадр находится левее или правее итоговой ширины распространения массива то 
            //итоговая ширина относительно центра влево =
            //длина левой границы + распространение влево (или заход кадра за него) + левая часть пользовательской строки
            string leftStencil  = DefaultSymbolLine((howDeepWeGo - 1) * leftMoving);
            string rightStencil = DefaultSymbolLine((howDeepWeGo - 1) * rightMoving);
            //центр изначальной строки
            center = CenterFinder(workPiece.Length);
            //Расстояние от центра пользовательской строки до края массива,
            //включает длину пользовательской строки до центра (=center), распространение, длину границы,
            //требуется для определения центра.
            int leftRangeToBorder = center + leftStencil.Length;
            //Дополнения слева/справа, чтобы пользовательский кадр вошёл.
            if (frameLeft < 0)
            {
                for (int i = leftRangeToBorder; i < Math.Abs(frameLeft); i++)
                    leftStencil = leftStencil + "0";

                leftRangeToBorder = center + leftStencil.Length;
            }

            if (frameRight > 0)
            {
                int rightRangeToEdge = workPiece.Length - (center + 1) + rightStencil.Length;
                for (int i = rightRangeToEdge; i < Math.Abs(frameRight); i++)
                    rightStencil = rightStencil + "0";
            }

            center = leftRangeToBorder + leftBorderLength;
            string layer = leftStencil + workPiece + rightStencil;
            return layer;
        }
    }
}
