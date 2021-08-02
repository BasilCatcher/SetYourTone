using System.Collections.Generic;
using SetYourTone.Models;
using System.Text;

namespace SetYourTone.Services
{
    public class VerbatimEndlessTopLayer : EndlessTopLayer
    {
        public VerbatimEndlessTopLayer(RuleModel RuleParameters, Dictionary<string, char> inputTriggers, string currentUserFrame)
            :base(RuleParameters, inputTriggers, currentUserFrame)
        { }
        protected override void LineCalculator(in char[] inputLine, out char[] outputLine)
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
                    //Режим буквальных триггеров
                    triggerLineSculpt.Append(inputLine[i + _RuleParameters.Offset + j]);
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
                    outputLine[k] = _RuleParameters.RightBorder[k - inputLine.GetLength(0) + _RuleParameters.RightBorder.Length];
                }
            }
        }
    }
}
