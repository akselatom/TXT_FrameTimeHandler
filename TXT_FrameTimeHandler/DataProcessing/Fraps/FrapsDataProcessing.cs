﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TXT_FrameTimeHandler.DataProcessing.Fraps
{
    public static class FrapsDataProcessing
    {
        public static Maybe<FrapsData> ProcessFrapsFile(string path)
        {
            FileStream fs = default;
            BufferedStream bs = default;
            StreamReader sr = default;
            try
            {
                var frapsData = new FrapsData();

                fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                bs = new BufferedStream(fs);
                sr = new StreamReader(bs);

                sr.ReadLine(); // read emmpty line
                sr.ReadLine(); // read first item

                var line = "";
                var lastItemFrameTime = 0.0; // first item frametime is always zero 

                frapsData.FrameTimes.AddLast(lastItemFrameTime);

                // others
                while ((line = sr.ReadLine()) != null)
                {
                    #region parse string of format "number, frametime"
                    var index = line.LastIndexOf(',');
                    line = line.Substring(index + 1);
                    line = string
                        .Join("", line.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                    #endregion

                    var currentItemFrameTime = Convert.ToDouble(line, 
                        CultureInfo.InvariantCulture // because Russians and others have "12,34", but not "12.34" :)
                        );
                    frapsData.FrameTimes.AddLast(currentItemFrameTime - lastItemFrameTime);
                    lastItemFrameTime = currentItemFrameTime;
                }


                return new Maybe<FrapsData>(frapsData);
            }
            catch (Exception)
            {
                if (fs != null)
                    fs.Dispose();
                if (bs != null)
                    bs.Dispose();
                if (sr != null)
                    sr.Dispose();

                return Maybe<FrapsData>.None;
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
                if (bs != null)
                    bs.Dispose();
                if (sr != null)
                    sr.Dispose();
            }
        }
    }
}