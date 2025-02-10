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

def generate_directory_tree(start_path: Path, skip_dirs: list) -> str:
    """Generate a directory tree string starting from start_path."""
    tree = []

    def recurse(path: Path, indent: str = "") -> None:
        if path.name in skip_dirs:
            return  # Skip specified directories

        try:
            # Add current directory name
            tree.append(f"{indent}{path.name}/")
            # Process each item in the directory
            for item in sorted(path.iterdir()):
                if item.is_dir():
                    recurse(item, indent + "  ")
        except PermissionError:
            tree.append(f"{indent}  ⚠️ Permission denied")
        except Exception as e:
            tree.append(f"{indent}  ⚠️ Error: {e}")

    # Start recursion with the start_path
    recurse(start_path)
    return "\n".join(tree)

def read_file_content(file_path: Path) -> str:
    """Read and return the content of a file as a string, handling errors."""
    try:
        with open(file_path, 'r', encoding='utf-8') as f:
            return f.read()
    except UnicodeDecodeError:
        return "[Binary or non-UTF-8 file content omitted]"
    except Exception as e:
        return f"[Error reading file: {e}]"

def main():
    try:
        project_root = find_project_root()
        target_dir = project_root / "server"
        output_dir = project_root / "shared" / "output"
        output_file = output_dir / "server_contents.txt"
        skip_dirs = ["bin", "obj", ".git", ".vs", "node_modules", "packages", "dist", "output"]

        # Ensure output directory exists
        output_dir.mkdir(parents=True, exist_ok=True)

        # Generate directory tree
        directory_tree = generate_directory_tree(target_dir, skip_dirs)

        # Collect file contents
        file_contents = []
        for file_path in target_dir.rglob('*'):
            # Skip unwanted directories
            if any(skip_dir in file_path.parts for skip_dir in skip_dirs):
                continue

            if file_path.is_file():
                relative_path = file_path.relative_to(target_dir)
                content = read_file_content(file_path)
                file_contents.append((relative_path, content))

        # Write to output file
        with open(output_file, 'w', encoding='utf-8') as f:
            f.write("=== Directory Tree ===\n")
            f.write(directory_tree)
            f.write("\n\n=== File Contents ===\n")

            for path, content in file_contents:
                f.write(f"\nFile: {file_path.parents} {path}\n")
                f.write("Content:\n")
                f.write(content)
                f.write("\n" + "-" * 50 + "\n")

        print(f"Successfully generated output at {output_file}")

    except Exception as e:
        print(f"Error: {e}", file=sys.stderr)
        sys.exit(1)

if __name__ == "__main__":
    main()
