using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Game.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.Scripts.Infra
{
    public class Utils
    {
        public static string CurrencyString(int value)
        {
            return value.ToString("N0");
        }

        public static float GetScreenRatio()
        {
            return (float) Screen.width / Screen.height;
        }

        public static int CalcPrice(float price, float baseValue, int round = 10)
        {
            var realPrice = (int)(baseValue * price);
            if (realPrice < round)
            {
                return realPrice;
            }
            
            realPrice = (realPrice / round) * round;
            return realPrice;
        }

        public static string TimeLeftString(long timeLeft, bool pad = false)
        {
            double sec_num = timeLeft;
            
            var days   = Math.Floor(sec_num / (24 * 3600));
            var d = days.ToString();
            if (days < 10 && pad)
            {
                d = "0" + d;
            }

            sec_num -= days * (24 * 3600);
            
            var hours   = Math.Floor(sec_num / 3600);
            var h = hours.ToString();
            if (hours < 10 && pad)
            {
                h = "0" + h;
            }
            var minutes = Math.Floor((sec_num - (hours * 3600)) / 60);
            var m = minutes.ToString();
            if (minutes < 10 && pad)
            {
                m = "0" + m;
            }
            var seconds = sec_num - (hours * 3600) - (minutes * 60);
            var s = seconds.ToString();
            if (seconds < 10 && pad)
            {
                s = "0" + s;
            }

            if (days > 0)
            {
                return d + "d " + h + "h " + m + "m ";    
            }
            return h + "h " + m + "m " + s + "s ";
        }
        
        public static string Pad(int num)
        {
            if (num < 10)
            {
                return "0" + num;
            }

            return "" + num;
        }
        
        public static string GetPriceString(int price)
        {
            if (price < 1000)
            {
                return "" + price;
            }

            var p = (float)price / 1000;
            var postfix = "K";
            if (p >= 1000)
            {
                postfix = "M";
                p /= 1000;
            }
            var str =  p.ToString("0.0") + postfix;
            return str.Replace(".0" + postfix, postfix);
        }

        public static float GetAspectRatioDelta()
        {
            var designAspectRatio = 1.778f;
            var aspectRatio = (float) Screen.width / Screen.height;
            return Mathf.Clamp01((aspectRatio - designAspectRatio) / 0.386f);
        }

        public static string TimeString(int time)
        {
            if (time < 0)
            {
                time = 0;
            }

            var hours = time / (60 * 60);
            time -= hours * (60 * 60);
            var minutes = time / 60;
            var seconds = time % 60;
            var minStr = minutes < 10 ? "0" + minutes : minutes.ToString();
            var secStr = seconds < 10 ? "0" + seconds : seconds.ToString();
            var hourStr = hours < 10 ? "0" + hours : hours.ToString();
            return $"{hourStr}:{minStr}:{secStr}";
        }
        
        public static string TimeStringMinSec(int time)
        {
            if (time < 0)
            {
                time = 0;
            }
            
            var minutes = time / 60;
            var seconds = time % 60;
            var minStr = minutes < 10 ? "0" + minutes : minutes.ToString();
            var secStr = seconds < 10 ? "0" + seconds : seconds.ToString();
            return $"{minStr}:{secStr}";
        }
        
        public static string TimeStringLetters(int time)
        {
            if (time < 0)
            {
                time = 0;
            }
            var minutes = time / 60;
            var seconds = time % 60;
            var minStr = minutes < 10 ? "0" + minutes : minutes.ToString();
            var secStr = seconds < 10 ? "0" + seconds : seconds.ToString();
            return $"{minStr}m {secStr}s";
        }

        public static string TimeStringLettersLong(int time)
        {
            if (time < 0)
            {
                time = 0;
            }

            var hours = time / (60 * 60);
            var hoursStr = hours < 10 ? "0" + hours : hours.ToString();

            time -= hours * (60 * 60);
            var minutes = time / 60;
            var seconds = time % 60;
            var minStr = minutes < 10 ? "0" + minutes : minutes.ToString();
            var secStr = seconds < 10 ? "0" + seconds : seconds.ToString();
            return $"{hoursStr}h {minStr}m {secStr}s";
        }

        public static int SecondsUntilMidnight()
        {
            var untilMidnight = DateTime.Today.AddDays (1.0) - DateTime.Now;
            return (int)untilMidnight.TotalSeconds;
        }

        public static DateTime Midnight()
        {
            return DateTime.Today.AddDays(1.0);
        }

        public static void CopyToClipboard(string s)
        {
            var te = new TextEditor {text = s};
            te.SelectAll();
            te.Copy();
        }

        public static string LoadTextFile(string fileName)
        {
            var txt = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
            if (txt == null)
            {
                Debug.Log("resource not found: " + fileName);
                return null;
            }
            return txt.text;
        }
        
        public static int RoundOff (int i, float scale)
        {
            var intScale = (int) scale;
            return ((int)Math.Round(i / scale)) * intScale;
        }
        
        public static void DisableLights(GameObject root)
        {
            var lights = root.GetComponentsInChildren<Light>();
            if (lights != null)
            {
                foreach (var l in lights)
                {
                    //Debug.Log("Disabled light: " + l);
                    l.gameObject.SetActive(false);
                }
            }
        }
        
        public static void FadeTo(GameObject root, float duration, float value, float delay = 0, Action onComplete = null)
        {
            var childs = root.GetComponentsInChildren<MaskableGraphic>();
            TweenerCore<Color, Color, ColorOptions> lastTween = null;
            foreach (var child in childs)
            {
                child.DOKill();
                lastTween = child.DOFade(value, duration).SetDelay(delay);
            }

            if (null != lastTween)
            {
                lastTween.OnComplete(() => { onComplete?.Invoke(); });
            }
        }
        
        public static void SetAlpha(MaskableGraphic image, float value)
        {
            var color = image.color;
            color.a = value;
            image.color = color;
        }
        
        public static void SetAlpha(SpriteRenderer image, float value)
        {
            var color = image.color;
            color.a = value;
            image.color = color;
        }
        
        public static void SetAlphaRec(GameObject root, float value)
        {
            var childs = root.GetComponentsInChildren<MaskableGraphic>();
            foreach (var child in childs)
            {
                SetAlpha(child, value);
            }
        }
        
        public static Sequence DoPumpAnimation(Transform o, float delay = 0, int loops = -1, float duration = 0.3f, float initialScale = 1.0f, float scaleFactor = 1.1f)
        {
            o.DOKill();
            
            var seq = DOTween.Sequence();
            seq.Append(o.transform.DOScale(initialScale * scaleFactor, duration));
            seq.Append(o.transform.DOScale(initialScale, duration));
            seq.SetLoops(loops);
            seq.SetDelay(delay);
            seq.Play();
            return seq;
        }

        public static void SnapGallery(int selectedRow, Transform content)
        {
            var rowOffsetIndex = Mathf.Max(0, selectedRow - 1);
            var selectedRowOffset = rowOffsetIndex * 250;
            if (rowOffsetIndex > 0)
            {
                selectedRowOffset += 100;
            }
            var rect = content.GetComponent<RectTransform>();
            var pos = rect.anchoredPosition;
            pos.y = selectedRowOffset;
            rect.anchoredPosition = pos;
        }

        public static string CreateRandomString(int len)
        {
            var result = "";
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            var charactersLength = characters.Length;
            for (var i = 0; i < len; i++) {
                result += characters[Random.Range(0, charactersLength)];
            }
            return result;
        }

        public static string GetLocale()
        {
            return ModelManager.Get().GlobalPref.Language.ToString();
            //return SystemLanguage.German.ToString();// Application.systemLanguage.ToString();//SystemLanguage.Spanish.ToString(); 
        }
        
        public static string GetEnergyDurationText(int durationSec)
        {
            var text = "";
            var hours = durationSec / (60 * 60);
            if (hours < 1)
            {
                var minutes = durationSec / 60;
                if (minutes < 60)
                {
                    text = durationSec + "s";
                }
                else
                {
                    text = minutes + "m";
                }
            }
            else
            {
                text = hours + "h";
            }

            return text;
        }

        public static string DateString(DateTime date)
        {
            var day = date.Day.ToString();
            if (date.Day < 10)
            {
                day = "0" + day;
            }

            var monthInt = date.Month + 1;
            var month = monthInt.ToString();
            if (monthInt < 10)
            {
                month = "0" + month;
            }

            var year = date.Year.ToString().Substring(2);

            return day + "-" + month + "-" + year;
        }
    }
}