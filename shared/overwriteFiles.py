import sys
from pathlib import Path

def find_project_root() -> Path:
    """Find the project root directory containing 'vitality-builder'."""
    current = Path(__file__).resolve().parent
    while current.name.lower() != 'vitality-builder' and current.parent != current:
        current = current.parent
    if current.name.lower() != 'vitality-builder':
        raise RuntimeError("Could not find project root ('vitality-builder' directory)")
    return current

def parse_input_file(input_path: Path) -> list[tuple[Path, str]]:
    """Parse the input file into a list of (relative_path, content) tuples."""
    try:
        with open(input_path, 'r', encoding='utf-8') as f:
            content = f.read()
    except FileNotFoundError:
        raise RuntimeError(f"Input file not found: {input_path}")

    # Split into individual file sections separated by dashes
    separator = '\n' + '-'*50 + '\n'
    file_sections = [s.strip() for s in content.split(separator) if s.strip()]

    files = []
    for section in file_sections:
        lines = section.split('\n')
        if len(lines) < 2 or not lines[0].startswith("File: ") or lines[1] != "Content:":
            print(f"Skipping invalid section:\n{section}")
            continue

        relative_path = Path(lines[0][6:])  # Remove "File: " prefix
        file_content = '\n'.join(lines[2:])  # Content starts after "Content:" line
        files.append((relative_path, file_content))
    
    return files

def overwrite_files(files: list[tuple[Path, str]], target_dir: Path) -> None:
    """Overwrite files in target directory with new contents."""
    for relative_path, content in files:
        full_path = target_dir / relative_path
        
        try:
            # Create parent directories if they don't exist
            full_path.parent.mkdir(parents=True, exist_ok=True)
            
            # Write new content to file
            with open(full_path, 'w', encoding='utf-8') as f:
                f.write(content)
            print(f"Successfully updated: {relative_path}")
            
        except Exception as e:
            print(f"Error updating {relative_path}: {e}")

def main():
    try:
        project_root = find_project_root()
        input_file = project_root / "shared" / "input" / "overwrite_files_input.txt"
        target_dir = project_root / "server"

        if not input_file.exists():
            raise RuntimeError(f"Input file not found: {input_file}")

        # Parse input file
        files_to_overwrite = parse_input_file(input_file)
        
        if not files_to_overwrite:
            print("No valid file entries found in input file")
            return

        # Overwrite files
        overwrite_files(files_to_overwrite, target_dir)

        print("\nFile overwrite process completed")

    except Exception as e:
        print(f"Error: {e}", file=sys.stderr)
        sys.exit(1)

if __name__ == "__main__":
    main()