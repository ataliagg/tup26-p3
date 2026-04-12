import subprocess
from pathlib import Path
from agents import Agent, Runner, ShellTool, function_tool

WORKSPACE = Path(__file__).resolve().parent

@function_tool
def read_file(path: str) -> str:
    return path.read_text(encoding="utf-8")

@function_tool
def write_file(path: str, content: str) -> str:
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(content, encoding="utf-8")
    return f"OK: {path}"

def run_shell(request) -> str:
    commands = getattr(getattr(request.data, "action", None), "commands", None)
    if commands is None:
        commands = getattr(request.data, "commands", [])

    outputs = []
    for cmd in commands:
        result = subprocess.run(cmd, shell=True, cwd=WORKSPACE, capture_output=True, text=True)
        outputs.append( f"$ {cmd}\nexit_code: {result.returncode}\n--- STDOUT ---\n{result.stdout.strip() or '(vacío)'}\n--- STDERR ---\n{result.stderr.strip() or '(vacío)'}" )
    return "\n\n".join(outputs)

agent = Agent(
    name="NanoProg",
    model="gpt-5.4",
    instructions=f"""
        Eres un asistente de programación experto en Python.
        Trabajás en {WORKSPACE}.
        Antes de modificar, leé archivos relevantes.
        Usá read_file/write_file para archivos y shell para comandos.
        No inventes resultados. Respondé corto.""",
    tools=[ShellTool(executor=run_shell), read_file, write_file],
)

def main():
    history = []
    while True:
        user_input = input("Tú> ").strip()
        if user_input.lower() in {"salir", "exit", "quit"}: break
        if not user_input: continue

        result = Runner.run_sync(agent, history + [{"role": "user", "content": user_input}])
        print(f"\nAgente> {result.final_output}\n")
        history = result.to_input_list()

if __name__ == "__main__":
    main()