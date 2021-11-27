### TF is this?

You're probably asking yourself what the hell this is

It's cmake or make, but worse!

### Why would I use it rather then something like cmake?

idk, you tell me

### What can it do?

Well, it has support for WSL so you can easily build on Windows using WSL...

And you can use multiple architectures...

### Does it work on Windows?

Yes!

### Does it work on Linux?

Not sure, I didn't test it, but probably not

### Does it run crysis?

What?

### How do I use it?

Well, first you should probably build it, using the dotnet SDK

After that you can add it to your PATH variable or just copy it in your project's directory

Then you need to make a json file called project.json and just copy paste this json data in there

(In this example I'm using the [i686-elf-tools](https://github.com/lordmilko/i686-elf-tools) to compile a OS Kernel,
if you want to try it out you can just copy paste all the files from [here](https://wiki.osdev.org/Bare_Bones) (and put the linker.ld in a directory called i686))

```json
{
    "profiles": {
        "ActiveProfile": "i686",
        "Profiles": [
            {
                "Name": "i686",
                "Compilers": [
                    {
                        "FilePattern": "*.c",
                        "File": "i686-elf-gcc",
                        "Arguments": "-c {in} -o {out} -std=gnu99 -ffreestanding -O2 -Wall -Wextra",
                        "UseWSL": true
                    },
                    {
                        "FilePattern": "*.s",
                        "File": "i686-elf-as",
                        "Arguments": "{in} -o {out}",
                        "UseWSL": true
                    }
                ],
                "linker": {
                    "File": "i686-elf-gcc",
                    "Arguments": "-T {linker} -o {out} -ffreestanding -O2 -nostdlib {in} -lgcc",
                    "LinkerFile": "i686/linker.ld",
                    "UseWSL": true
                },
                "Directories": [
                    "i686"
                ]
            }
        ]
    }
}
```

And now all you have to do is open a terminal and type in batoss

After that you should be presented with a `i686.bin` file in the bin directory