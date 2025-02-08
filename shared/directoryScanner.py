from pathlib import Path
import os
from typing import Generator, List, Union
from dataclasses import dataclass
from datetime import datetime
import sys

# Configuration
Scan_Specified_Directory = 1
Max_Directory_Depth = 2  # Controls how many levels deep the directory scan will go

@dataclass
class FileInfo:
    """Store information about a file or directory."""
    path: Path
    is_dir: bool
    size: int
    modified_time: datetime

def get_directory_contents_pathlib(
    directory: Union[str, Path],
    pattern: str = "*",
    recursive: bool = True
) -> Generator[FileInfo, None, None]:
    """Get contents of a directory using pathlib."""
    root_path = Path(directory)
    if not root_path.exists():
        raise FileNotFoundError(f"Directory not found: {directory}")

    glob_func = root_path.rglob if recursive else root_path.glob

    for path in glob_func(pattern):
        try:
            stat = path.stat()
            yield FileInfo(
                path=path,
                is_dir=path.is_dir(),
                size=stat.st_size,
                modified_time=datetime.fromtimestamp(stat.st_mtime)
            )
        except (PermissionError, OSError) as e:
            print(f"Error accessing {path}: {e}")

def print_directory_tree(
    directory: Union[str, Path],
    indent: str = "",
    file=sys.stdout,
    current_depth: int = 0
) -> None:
    """
    Print a directory tree structure to the specified output (console or file).
    Now respects maximum depth setting.
    """
    try:
        root = Path(directory)
        print(f"{indent} {root.name}/", file=file)

        # Don't recurse if we've reached maximum depth
        if current_depth >= Max_Directory_Depth:
            print(f"{indent}   [...deeper contents omitted...]", file=file)
            return

        for item in sorted(root.iterdir()):
            if item.is_dir():
                print_directory_tree(item, indent + "  ", file=file, current_depth=current_depth + 1)
            else:
                print(f"{indent}   {item.name}", file=file)
    except PermissionError:
        print(f"{indent}  ⚠️ Permission denied", file=file)
    except Exception as e:
        print(f"{indent}  ⚠️ Error: {e}", file=file)

def find_project_root() -> Path:
    """Find the project root directory containing 'vitality-builder'."""
    current = Path(__file__).resolve().parent
    while current.name.lower() != 'vitality-builder' and current.parent != current:
        current = current.parent
    if current.name.lower() != 'vitality-builder':
        raise RuntimeError("Could not find project root ('vitality-builder' directory)")
    return current

def ensure_output_directory(output_dir: Path) -> None:
    """Ensure the output directory exists, create if it doesn't."""
    output_dir.mkdir(parents=True, exist_ok=True)

def main():
    try:
        # Find the project root and define paths
        project_root = find_project_root()
        output_dir = project_root / "shared" / "output"
        output_file = output_dir / "directory.txt"

        # Ensure the output directory exists
        ensure_output_directory(output_dir)

        print(f"Project root: {project_root}")
        print(f"Output directory: {output_dir}")
        print(f"Output file: {output_file}")
        print(f"Maximum directory depth: {Max_Directory_Depth}")

        # Open the output file
        with open(output_file, 'w', encoding='utf-8') as f:
            if Scan_Specified_Directory == 1:
                # Use project root as the directory to scan
                directory_to_scan = project_root
                print(f"Scanning directory: {directory_to_scan}\n")
                print(f"Scanning directory: {directory_to_scan}\n", file=f)
                current_dir = directory_to_scan
            else:
                # Get the current working directory
                current_dir = Path.cwd()
                print(f"Scanning directory: {current_dir}\n")
                print(f"Scanning directory: {current_dir}\n", file=f)

            print("=== Directory Tree ===", file=f)
            print_directory_tree(current_dir, file=f)

            print("\n=== Python Files ===", file=f)

            # Find and print information about all Python files
            python_files = get_directory_contents_pathlib(current_dir, "*.py")
            for file_info in python_files:
                if not file_info.is_dir:  # Skip directories, only show files
                    output = f"\nFile: {file_info.path.name}\n"
                    output += f"Size: {file_info.size:,} bytes\n"
                    output += f"Modified: {file_info.modified_time.strftime('%Y-%m-%d %H:%M:%S')}"
                    print(output, file=f)

        print(f"\nDirectory information has been written to {output_file}")
    except Exception as e:
        print(f"Error: {e}")
        raise

if __name__ == "__main__":
    main()
