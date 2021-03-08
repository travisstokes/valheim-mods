using System;
using System.Collections.Generic;
using System.Linq;

namespace OVC.GameMods.Valheim.ShippableMetal
{
    public class RotatingMenuManager<T>
    {
        private Func<IEnumerable<T>> _optionLookup;
        private Func<T, bool> _optionSelector;
        private Func<T, string> _optionFormatter;
        private string _currentFormattedOption;

        public RotatingMenuManager(Func<IEnumerable<T>> optionLookup, Func<T, bool> optionSelector, Func<T, string> optionFormatter)
        {
            _optionLookup = optionLookup;
            _optionSelector = optionSelector;
            _optionFormatter = optionFormatter;
        }

        public MenuManagerOptionState GetCurrentOptionState()
        {
            var options = GetFormattedOptions();
            var currentOptionExists = !string.IsNullOrEmpty(_currentFormattedOption) && options.Contains(_currentFormattedOption);
            _currentFormattedOption = currentOptionExists ? _currentFormattedOption : GotoNext();

            return new MenuManagerOptionState()
            {
                Option = _currentFormattedOption,
                TotalOptionsAvailable = options.Count()
            };
        }

        public string GetCurrentOption()
        {
            return GetCurrentOptionState()?.Option;
        }

        private IOrderedEnumerable<string> GetFormattedOptions()
        {
            return _optionLookup()
                .Where(_optionSelector)
                .Select(_optionFormatter)
                .Distinct()
                .OrderBy(i => i);
        }

        public string GotoNext()
        {
            var options = GetFormattedOptions();

            if (string.IsNullOrWhiteSpace(_currentFormattedOption))
            {
                _currentFormattedOption = options.FirstOrDefault();
            }
            else
            {
                _currentFormattedOption = options.SkipWhile(i => string.CompareOrdinal(i, _currentFormattedOption) < 1).FirstOrDefault() ?? options.FirstOrDefault();
            }

            return _currentFormattedOption;
        }
    }

    public class MenuManagerOptionState
    {
        public string Option { get; set; }
        public int TotalOptionsAvailable { get; set; }
    }

}
