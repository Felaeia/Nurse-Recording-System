import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import '../widgets/app_background.dart';
import '../widgets/custom_button.dart';
import '../widgets/app_colors.dart';
import '../mock/mock_patient_record_db.dart';

class PatientRecords extends StatefulWidget {
  const PatientRecords({super.key});

  @override
  State<PatientRecords> createState() => _PatientRecordsState();
}

class _PatientRecordsState extends State<PatientRecords> {
  final _formKey = GlobalKey<FormState>();

  final _recordPart1Controller = TextEditingController();
  final _recordPart2Controller = TextEditingController();
  final _recordPart3Controller = TextEditingController();
  final _recordPart4Controller = TextEditingController();

  final _patientPart1Controller = TextEditingController();
  final _patientPart2Controller = TextEditingController();
  final _patientPart3Controller = TextEditingController();
  final _patientPart4Controller = TextEditingController();

  final _dateController = TextEditingController();
  final _diagnosisController = TextEditingController();
  final _symptomsController = TextEditingController();
  final _treatmentController = TextEditingController();
  final _notesController = TextEditingController();

  final _db = MockPatientRecordDb();

  @override
  void dispose() {
    _recordPart1Controller.dispose();
    _recordPart2Controller.dispose();
    _recordPart3Controller.dispose();
    _recordPart4Controller.dispose();
    _patientPart1Controller.dispose();
    _patientPart2Controller.dispose();
    _patientPart3Controller.dispose();
    _patientPart4Controller.dispose();
    _dateController.dispose();
    _diagnosisController.dispose();
    _symptomsController.dispose();
    _treatmentController.dispose();
    _notesController.dispose();
    super.dispose();
  }

  String getFormattedRecordID() {
    return 'R${_recordPart1Controller.text.padRight(2, '_')}-'
        '${_recordPart2Controller.text.padRight(2, '_')}-'
        '${_recordPart3Controller.text.padRight(4, '_')}-REC'
        '${_recordPart4Controller.text.padRight(3, '_')}';
  }

  String getFormattedPatientID() {
    return 'C${_patientPart1Controller.text.padRight(2, '_')}-'
        '${_patientPart2Controller.text.padRight(2, '_')}-'
        '${_patientPart3Controller.text.padRight(4, '_')}-MAN'
        '${_patientPart4Controller.text.padRight(3, '_')}';
  }

  void _submitForm() {
    if (_formKey.currentState?.validate() ?? false) {
      final record = PatientRecord(
        recordID: getFormattedRecordID(),
        patientID: getFormattedPatientID(),
        date: _dateController.text,
        diagnosis: _diagnosisController.text,
        symptoms: _symptomsController.text,
        treatment: _treatmentController.text,
        notes: _notesController.text,
      );

      _db.addRecord(record);
      _db.printAllRecords();

      showDialog(
        context: context,
        builder: (context) => AlertDialog(
          title: const Text('Success'),
          content: const Text('Patient record saved to mock DB!'),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: const Text('OK'),
            ),
          ],
        ),
      );

      _formKey.currentState?.reset();
      _recordPart1Controller.clear();
      _recordPart2Controller.clear();
      _recordPart3Controller.clear();
      _recordPart4Controller.clear();
      _patientPart1Controller.clear();
      _patientPart2Controller.clear();
      _patientPart3Controller.clear();
      _patientPart4Controller.clear();
      _dateController.clear();
      _diagnosisController.clear();
      _symptomsController.clear();
      _treatmentController.clear();
      _notesController.clear();
      setState(() {});
    }
  }

  Widget _buildIDSegment(TextEditingController controller, int length) {
    return SizedBox(
      width: length * 18.0,
      child: TextFormField(
        controller: controller,
        inputFormatters: [
          FilteringTextInputFormatter.digitsOnly,
          LengthLimitingTextInputFormatter(length),
        ],
        keyboardType: TextInputType.number,
        decoration: const InputDecoration(
          isDense: true,
          contentPadding: EdgeInsets.symmetric(vertical: 8),
          border: UnderlineInputBorder(),
        ),
        style: const TextStyle(fontSize: 18),
        validator: (value) {
          if (value == null || value.length != length) return '';
          return null;
        },
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: AppBackground(
        child: SafeArea(
          child: Padding(
            padding: const EdgeInsets.all(16.0),
            child: SingleChildScrollView(
              child: Form(
                key: _formKey,
                child: Column(
                  children: [
                    const SizedBox(height: 20),
                    const Text(
                      'Patient Record Form',
                      style: TextStyle(
                        fontSize: 26,
                        fontWeight: FontWeight.bold,
                        color: Colors.black87,
                      ),
                    ),
                    const SizedBox(height: 25),

                    Container(
                      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                      decoration: BoxDecoration(
                        color: Colors.white,
                        borderRadius: BorderRadius.circular(15),
                      ),
                      child: Row(
                        children: [
                          ShaderMask(
                            shaderCallback: (bounds) =>
                                AppColors.primaryGradient.createShader(bounds),
                            child: const Icon(Icons.badge, color: Colors.white),
                          ),
                          const SizedBox(width: 12),
                          const Text('R', style: TextStyle(fontSize: 18)),
                          const SizedBox(width: 4),
                          _buildIDSegment(_recordPart1Controller, 2),
                          const Text('-', style: TextStyle(fontSize: 18)),
                          _buildIDSegment(_recordPart2Controller, 2),
                          const Text('-', style: TextStyle(fontSize: 18)),
                          _buildIDSegment(_recordPart3Controller, 4),
                          const Text('-REC', style: TextStyle(fontSize: 18)),
                          _buildIDSegment(_recordPart4Controller, 3),
                        ],
                      ),
                    ),

                    const SizedBox(height: 20),
                    Container(
                      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                      decoration: BoxDecoration(
                        color: Colors.white,
                        borderRadius: BorderRadius.circular(15),
                      ),
                      child: Row(
                        children: [
                          ShaderMask(
                            shaderCallback: (bounds) =>
                                AppColors.primaryGradient.createShader(bounds),
                            child: const Icon(Icons.badge, color: Colors.white),
                          ),
                          const SizedBox(width: 12),
                          const Text('C', style: TextStyle(fontSize: 18)),
                          const SizedBox(width: 4),
                          _buildIDSegment(_patientPart1Controller, 2),
                          const Text('-', style: TextStyle(fontSize: 18)),
                          _buildIDSegment(_patientPart2Controller, 2),
                          const Text('-', style: TextStyle(fontSize: 18)),
                          _buildIDSegment(_patientPart3Controller, 4),
                          const Text('-MAN', style: TextStyle(fontSize: 18)),
                          _buildIDSegment(_patientPart4Controller, 3),
                        ],
                      ),
                    ),

                    const SizedBox(height: 20),
                    _buildInputField(Icons.calendar_today, 'Date', controller: _dateController, requiredField: true, hintText: 'YYYY-MM-DD'),
                    _buildInputField(Icons.medical_services, 'Diagnosis', controller: _diagnosisController, requiredField: true),
                    _buildInputField(Icons.list_alt, 'Symptoms', controller: _symptomsController, requiredField: true),
                    _buildInputField(Icons.healing, 'Treatment', controller: _treatmentController),
                    _buildInputField(Icons.note, 'Notes', controller: _notesController, hintText: 'Any additional notes...'),

                    const SizedBox(height: 30),
                    CustomButton(text: 'Submit', onPressed: _submitForm),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildInputField(
    IconData icon,
    String label, {
    TextEditingController? controller,
    bool obscure = false,
    bool requiredField = false,
    String? hintText,
    List<TextInputFormatter>? inputFormatters,
    String? Function(String?)? validator,
    TextInputType? keyboardType,
  }) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8),
      child: Container(
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(15),
        ),
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
          child: TextFormField(
            controller: controller,
            obscureText: obscure,
            inputFormatters: inputFormatters,
            keyboardType: keyboardType ?? TextInputType.text,
            validator: validator ?? (requiredField ? (v) => (v == null || v.trim().isEmpty) ? 'Required' : null : null),
            decoration: InputDecoration(
              prefixIcon: ShaderMask(
                shaderCallback: (bounds) => AppColors.primaryGradient.createShader(bounds),
                child: Icon(icon, color: Colors.white),
              ),
              hintText: hintText,
              labelText: label,
              border: InputBorder.none,
            ),
          ),
        ),
      ),
    );
  }
}
