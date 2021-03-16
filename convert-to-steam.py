import glob


OPENTTD_TO_STEAM = {
    "arabic_egypt": "Arabic",
    "bulgarian": "Bulgarian",
    "simplified_chinese": "Chinese (Simplified)",
    "traditional_chinese": "Chinese (Traditional)",
    "czech": "Czech",
    "danish": "Danish",
    "dutch": "Dutch",
    "english": "English",
    "finnish": "Finnish",
    "french": "French",
    "german": "German",
    "greek": "Greek",
    "hungarian": "Hungarian",
    "italian": "Italian",
    "japanese": "Japanese",
    "korean": "Korean",
    "norwegian_bokmal": "Norwegian",
    "polish": "Polish",
    "portuguese": "Portuguese",
    "brazilian_portuguese": "Portuguese-Brazil",
    "romanian": "Romanian",
    "russian": "Russian",
    "spanish": "Spanish-Spain",
    "spanish_MX": "Spanish-Latin America",
    "swedish": "Swedish",
    "thai": "Thai",
    "turkish": "Turkish",
    "ukrainian": "Ukrainian",
    "vietnamese": "Vietnamese",
}


def convert(base_strings, data):
    """Create the Steam Store Page blob only if all strings are translated."""
    strings = []
    result = []
    for line in data.split("\n"):
        if not line or line[0] == "#":
            continue

        string_id, _, text = line.partition(":")
        string_id = string_id.strip()
        text = text.strip()

        if string_id.endswith("TITLE"):
            text = f"[h2]{text}[/h2]"

        strings.append(string_id)
        result.append(text)

    # Not everything is translated; we cannot generate a valid result
    if base_strings != strings:
        return None

    return "\n".join(result)


def find_base_string():
    """Find all strings in the base language."""
    base_strings = []
    with open("lang/english.txt") as fp:
        for line in fp.read().split("\n"):
            if not line or line[0] == "#":
                continue

            string_id, _, _ = line.partition(":")
            string_id = string_id.strip()
            base_strings.append(string_id)

    return base_strings


base_strings = find_base_string()

for filename in glob.glob("lang/*.txt"):
    language = filename.split("/")[1].split(".")[0]
    if language == "english":
        continue

    with open(filename) as fp:
        data = fp.read()

    translation = convert(base_strings, data)

    if translation:
        print("---")
        print(OPENTTD_TO_STEAM[language])
        print("---")
        print(translation)
        print("")

        input("Press any key to go to the next language ...")
