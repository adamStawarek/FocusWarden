namespace FocusWarden.Lib.Helpers.Interfaces
{
    using System.Collections.Generic;

    public interface IMotivationSentenceProvider
    {
        IEnumerable<string> GetMotivationSentences();
    }
}