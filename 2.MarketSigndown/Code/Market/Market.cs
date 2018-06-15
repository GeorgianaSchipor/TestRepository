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

            float totalWrittenLine = underwriters.Sum(u => u.WrittenLine);
            foreach (Underwriter u in underwriters)
            {
                float possibleSigned = (float)Math.Round((double)(u.WrittenLine * (TotalOrder / totalWrittenLine)), 2);
                if (u.MinLine <= possibleSigned)
                    u.SignedLine = possibleSigned;
            }

            if (totalMinLine != 0)
            {
                ManageUnderWritersWithMin(TotalOrder, underwriters);

            }


            return underwriters;
        }

        private static void ManageUnderWritersWithMin(float TotalOrder, List<Underwriter> underwriters)
        {
            var notSignedYet = underwriters.Where(u => u.SignedLine == 0).ToList();
            if (notSignedYet.Count() != 0)
            {
                if (notSignedYet.Count() == 2)
                {
                    float rest = 0;
                    foreach (Underwriter u in underwriters.OrderBy(u => u.SignedLine))
                    {
                        if (u.SignedLine == 0)
                        {
                            u.SignedLine = u.MinLine;
                            rest = TotalOrder - u.MinLine;
                        }
                        else
                        {
                            u.SignedLine = rest;
                        }
                    }

                }

                if (notSignedYet.Count() == 1)
                {

                }

            }
        }
    }
}
