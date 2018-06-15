using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Market
{
    public class Market
    {
        public List<Underwriter> MarketSigndown(float TotalOrder, List<Underwriter> underwriters)
        {
            int underwritersNr = underwriters.Count();
            if (underwritersNr == 0)
                return underwriters;

            float totalMinLine = underwriters.Sum(u => u.MinLine);
            if (underwriters == null || (totalMinLine > 0 && totalMinLine > TotalOrder))
                return null;

            bool blockedCount = true;
            while (blockedCount == true && underwriters.Where(u => u.Blocked == false).Count() > 0)
            {
                float restTotalOrder = TotalOrder
                    - underwriters.Where(u => u.Blocked == true).Sum(u => u.SignedLine);
                underwriters = GetSignedLines(underwriters, restTotalOrder, out blockedCount);
            }


            return underwriters;
        }

        public List<Underwriter> GetSignedLines(List<Underwriter> underwriters, float TotalOrder, out bool BlockedCount)
        {
            BlockedCount = false;
            float totalWrittenLine = underwriters.Where(u => u.Blocked == false).Sum(u => u.WrittenLine);

            foreach (Underwriter u in underwriters.Where(u => u.Blocked == false))
            {
                float signedLine = GetSignedLineNoMins(TotalOrder, u.WrittenLine, totalWrittenLine);
                if (u.MinLine <= signedLine)
                    u.SignedLine = signedLine;
                else
                {
                    u.SignedLine = u.MinLine;
                    u.Blocked = true;
                    BlockedCount = true;
                }
            }

            return underwriters;
        }


        private float GetSignedLineNoMins(float TotalOrder, float WrittenLine, float TotalWrittenLine)
        {
            return (float)Math.Round((double)(WrittenLine * (TotalOrder / TotalWrittenLine)), 2);
        }
    }
}
