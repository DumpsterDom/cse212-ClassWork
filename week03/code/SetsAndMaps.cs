using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;


public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        // Put all words into a HashSet
        HashSet<string> wordSet = new HashSet<string>(words);

        // Use another HashSet to store unique formatted pairs
        // prevents "am & ma" and "ma & am" from both appearing
        HashSet<string> pairs = new HashSet<string>();

        foreach (string word in words)
        {
            // Compute the reverse 
            string reverse = $"{word[1]}{word[0]}"; 

            // Skip if it's a palindrome 
            if (word == reverse) continue;

            // Check if the reverse actually exists in the original list
            if (wordSet.Contains(reverse))
            {
                // Create consistent ordering
                string pair = word.CompareTo(reverse) < 0
                    ? $"{word} & {reverse}"
                    : $"{reverse} & {word}";

                // Add to set
                pairs.Add(pair);
            }
        }

        // Convert back to array and return
        return pairs.ToArray();
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    /// <returns>fixed array of divisors</returns>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();

        // Read the file line by line
        foreach (var line in File.ReadLines(filename))
        {
            // Split the line by commas to get the columns
            var fields = line.Split(',');

            // Check that there are at least 4 columns as 0-based index 3 is column 4
            if (fields.Length >= 4)
            {
                // Get the degree from column 4 index 3
                string degree = fields[3].Trim();

                // Only count non-empty degrees
                if (!string.IsNullOrEmpty(degree))
                {
                    // If we've seen this degree before, increment the count
                    if (degrees.ContainsKey(degree))
                    {
                        degrees[degree]++;
                    }
                    // First time seeing this degree - start the count at 1
                    else
                    {
                        degrees[degree] = 1;
                    }
                }
            }
        }

        // Return the summary
        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  An anagram
    /// is when the same letters in a word are re-organized into a 
    /// new word.  A dictionary is used to solve the problem.
    /// 
    /// Examples:
    /// is_anagram("CAT","ACT") would return true
    /// is_anagram("DOG","GOOD") would return false because GOOD has 2 O's
    /// 
    /// Important Note: When determining if two words are anagrams, you
    /// should ignore any spaces.  You should also ignore cases.  For 
    /// example, 'Ab' and 'Ba' should be considered anagrams
    /// 
    /// Reminder: You can access a letter by index in a string by 
    /// using the [] notation.
    /// </summary>
public static bool IsAnagram(string word1, string word2)
    {
        // Remove spaces and convert to lowercase
        string clean1 = new string(word1.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToLower();
        string clean2 = new string(word2.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToLower();

        // Different lengths → cannot be anagrams
        if (clean1.Length != clean2.Length)
            return false;

        // Count letters in first word
        var charCount = new Dictionary<char, int>();

        foreach (char c in clean1)
        {
            if (charCount.ContainsKey(c))
                charCount[c]++;
            else
                charCount[c] = 1;
        }

        // Subtract counts from second word
        foreach (char c in clean2)
        {
            if (!charCount.ContainsKey(c) || charCount[c] == 0)
                return false;
            charCount[c]--;
        }

        // we made it here, all counts matched
        return true;
    }
    /// <summary>
    /// This function will read JSON (Javascript Object Notation) data from the 
    /// United States Geological Service (USGS) consisting of earthquake data.
    /// The data will include all earthquakes in the current day.
    /// 
    /// JSON data is organized into a dictionary. After reading the data using
    /// the built-in HTTP client library, this function will return a list of all
    /// earthquake locations ('place' attribute) and magnitudes ('mag' attribute).
    /// Additional information about the format of the JSON data can be found 
    /// at this website:  
    /// 
    /// https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson.php
    /// 
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        // Process the features
        var summary = new List<string>();

        foreach (var feature in featureCollection.Features)
        {
            var props = feature.Properties;

            // If we have both place and magnitude
            if (props.Mag.HasValue && !string.IsNullOrWhiteSpace(props.Place))
            {
                string entry = $"{props.Place} - Mag {props.Mag.Value:F2}";
                summary.Add(entry);
            }
        }

        // Return as array
        return summary.ToArray();
    }
}