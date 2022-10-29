import glob
import os


OPENTTD_TO_STEAM = {
    "arabic_egypt": "arabic",
    "bulgarian": "bulgarian",
    "simplified_chinese": "schinese",
    "traditional_chinese": "tchinese",
    "czech": "czech",
    "danish": "danish",
    "dutch": "dutch",
    "english": "english",
    "finnish": "finnish",
    "french": "french",
    "german": "german",
    "greek": "greek",
    "hungarian": "hungarian",
    "italian": "italian",
    "japanese": "japanese",
    "korean": "koreana",
    "norwegian_bokmal": "norwegian",
    "polish": "polish",
    "portuguese": "portuguese",
    "brazilian_portuguese": "brazilian",
    "romanian": "romanian",
    "russian": "russian",
    "spanish": "spanish",
    "spanish_MX": "latam",
    "swedish": "swedish",
    "thai": "thai",
    "turkish": "turkish",
    "ukrainian": "ukrainian",
    "vietnamese": "vietnamese",
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

with open("steam-template.json") as fp:
    template = fp.read()

os.makedirs("steam", exist_ok=True)

for filename in glob.glob("lang/*.txt"):
    language = filename.split("/")[1].split(".")[0]
    if language == "english":
        continue

    with open(filename) as fp:
        data = fp.read()

    translation = convert(base_strings, data)

    if translation:
        # Replace some characters as JSON / Steam wants them slightly different
        translation = translation.replace('"', "&quot;").replace("/", "\\/")

        # Create the description from the first line of the translation.
        description = translation.split("\n")[0]

        # Change the newlines into something more Steam-like.
        translation = translation.replace("\n", "\\r\\n")

        # But do not exceeded 300 characters and still have a full sentence.
        if len(description) > 300:
            print(f"Description too long, truncating {language}")
            if language == "thai":
                # Thai has a different sentence structure; so we cut it on a specific part.
                description = description[0 : description.find(" เกมนี้เป็นเกมแบบ")]
            elif "。" in description:
                # Languages like Japanese have a different "end of sentence" character.
                description = description.rsplit("。", 1)[0] + "。"
            else:
                description = description.rsplit(".", 2)[0] + "."

        if len(description) > 300:
            print(f"Description still too long for {language}!")

        # Generate the JSON file based on the template.
        with open(f"steam/storepage_343360_{OPENTTD_TO_STEAM[language]}.json", "w") as fp:
            fp.write(
                template.format(
                    language=OPENTTD_TO_STEAM[language],
                    about=translation,
                    short_description=description,
                )
            )
