using System;
using System.Collections.Generic;
using FocusWarden.Lib.Helpers.Interfaces;

namespace FocusWarden.Lib.Helpers
{
    internal class MotivationSentencesProvider : IMotivationSentenceProvider
    {
        private static readonly Lazy<IEnumerable<string>> MotivationSentences =
            new Lazy<IEnumerable<string>>(() => new List<string>
            {
                "Success consists of going from failure to failure without loss of enthusiasm.",
                "Our greatest weakness lies in giving up. The most certain way to succeed is always to try just one more time.",
                "Believe in yourself and all that you are. Know that there is something inside you that is greater than any obstacle.",
                "There are far, far better things ahead than any we leave behind.",
                "Sleep is for weaklings.",
                "Keep it up!",
                "You may only succeed if you desire succeeding; you may only fail if you do not mind failing.",
                "Develop success from failures. Discouragement and failure are two of the surest stepping stones to success.",
                "To be successful you must accept all challenges that come your way. You can’t just accept the ones you like.",
                "Success doesn’t come to you, you’ve got to go to it.",
                "If you want to achieve excellence, you can get there today. As of this second, quit doing less-than-excellent work.",
                "Failure will never overtake me if my determination to succeed is strong enough.",
                "Success means having the courage, the determination, and the will to become the person you believe you were meant to be.",
                "Success is no accident. It is hard work, perseverance, learning, studying, sacrifice, and most of all, love of what you are doing or learning to do.",
                "The secret of your success is determined by your daily agenda.",
                "Don’t let what you cannot do interfere with what you can do.",
                "The difference between a successful person and others is not a lack of strength, not a lack of knowledge, but rather a lack in will.",
            });

        public IEnumerable<string> GetMotivationSentences()
        {
            return MotivationSentences.Value;
        }
    }
}
