namespace QApp.Qsharp  {

    open Microsoft.Quantum.Intrinsic;
    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Measurement;


    @EntryPoint()
    operation SampleRandomNumberInRange(nQubits : Int) : Result[] {
        // create a register of n qubits
        use qubits = Qubit[nQubits];
        //put each qubit in superposition
        ApplyToEach(H, qubits);
        // measure all qubits and return the result
        return MultiM(qubits);
    }
}
