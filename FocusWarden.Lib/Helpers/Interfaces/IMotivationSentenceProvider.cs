using System.Collections.Generic;

namespace FocusWarden.Lib.Helpers.Interfaces
{
    public interface IMotivationSentenceProvider
    {
        IEnumerable<string> GetMotivationSentences();
    }
}