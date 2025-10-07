using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace fileorganizer;

public class Loader
{

	// source folder
	string path = "/home/azz/Downloads";
	// queried filetypes
	string[] mediatypes = { "*.mp4", "*.mov", "*.mkv", "*.webm" };

	// function to load all the specified filetypes into a list
	public List<string> Load()
	{
		List<string> result = new List<string>();

		foreach (var f in mediatypes)
		{
			string[] dirget = Directory.GetFiles(path, f);
			foreach (var fil in dirget)
			{
				result.Add(fil);
			}
		}
		return result;
	}

}

// purely a debugging tool here
// public class Fixer
// {
//
// 	string path = "/home/azz/Memes/";
// 	string dest = "/home/azz/Downloads/";
//
// 	public void Fix()
// 	{
// 		foreach (var dir in Directory.EnumerateDirectories(path))
// 		{
// 			var name = Path.GetFileName(dir);
// 			if (name.Length == 3)
// 			{
// 				foreach (var source in Directory.EnumerateFiles(dir))
// 				{
// 					var filename = Path.GetFileName(source);
// 					var dstPath = Path.Combine(dest, filename);
//
// 					File.Move(source, dstPath);
// 				}
//
// 				Directory.Delete(dir, false);
// 			}
// 		}
// 	}
// }

class Program
{
	static void Main(string[] args)
	{
		//load the files
		var loader = new Loader();

		var list = loader.Load();

		// var fixer = new Fixer();
		// fixer.Fix();
		foreach (var k in list)
		{
			//filter out potential movies or series/anime episodes (>100mb)
			FileInfo f = new FileInfo(k);
			if (f.Length > 100000000)
			{
				Console.WriteLine($"{f} size is {f.Length} bytes, probably not a meme video, moving it to /movies.");
				if (!Directory.Exists(@"/home/azz/Movies")) Directory.CreateDirectory(@"/home/azz/Movies");
				string fullpath = Path.Combine("/home/azz/Movies/", f.Name);
				f.MoveTo(fullpath);
			}
			//move the rest into memes folder, with each file going into a folder named after its download date
			else
			{
				string yoinkedAt = $"{f.CreationTime.Year.ToString()}-{f.CreationTime.Month.ToString()}-{f.CreationTime.Day.ToString()}";
				if (!Directory.Exists(@$"/home/azz/Memes/{yoinkedAt}")) Directory.CreateDirectory(@$"/home/azz/Memes/{yoinkedAt}");
				string fullpath = Path.Combine(@$"/home/azz/Memes/{yoinkedAt}", f.Name);
				f.MoveTo(fullpath);
			}

		}
	}
}
