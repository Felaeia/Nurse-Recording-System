import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import '../widgets/app_background.dart';
import '../widgets/custom_button.dart';
import '../widgets/app_colors.dart';
import '../mock/mock_nurse_form_db.dart';

class AddForm extends StatefulWidget {
  const AddForm({super.key});

  @override
  State<AddForm> createState() => _AddFormState();
}

class _AddFormState extends State<AddForm> {
  final _formKey = GlobalKey<FormState>();

  final _idPart1Controller = TextEditingController();
  final _idPart2Controller = TextEditingController();
  final _idPart3Controller = TextEditingController();
  final _idPart4Controller = TextEditingController();

  final _firstNameController = TextEditingController();
  final _middleNameController = TextEditingController();
  final _lastNameController = TextEditingController();
  final _addressController = TextEditingController();
  final _passwordController = TextEditingController();
  final _facebookController = TextEditingController();
  final _emailController = TextEditingController();
  final _emergencyContactController = TextEditingController();

  @override
  void dispose() {
    _idPart1Controller.dispose();
    _idPart2Controller.dispose();
    _idPart3Controller.dispose();
    _idPart4Controller.dispose();
    _firstNameController.dispose();
    _middleNameController.dispose();
    _lastNameController.dispose();
    _addressController.dispose();
    _passwordController.dispose();
    _facebookController.dispose();
    _emailController.dispose();
    _emergencyContactController.dispose();
    super.dispose();
  }

  String getFormattedID() {
    return 'C${_idPart1Controller.text.padRight(2, '_')}-'
        '${_idPart2Controller.text.padRight(2, '_')}-'
        '${_idPart3Controller.text.padRight(4, '_')}-MAN'
        '${_idPart4Controller.text.padRight(3, '_')}';
  }

  void _submitForm() {
    if (_formKey.currentState?.validate() ?? false) {
      final form = NurseForm(
        id: getFormattedID(),
        firstName: _firstNameController.text,
        middleName: _middleNameController.text,
        lastName: _lastNameController.text,
        address: _addressController.text,
        password: _passwordController.text,
        facebook: _facebookController.text,
        email: _emailController.text,
        emergencyContact: _emergencyContactController.text,
      );

      final db = MockNurseFormDb();
      db.addForm(form);
      db.printAllForms();

      showDialog(
        context: context,
        builder: (context) => AlertDialog(
          title: const Text('Success'),
          content: const Text('Patient form saved to mock DB!'),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: const Text('OK'),
            ),
          ],
        ),
      );

      _formKey.currentState?.reset();
      _idPart1Controller.clear();
      _idPart2Controller.clear();
      _idPart3Controller.clear();
      _idPart4Controller.clear();
      _firstNameController.clear();
      _middleNameController.clear();
      _lastNameController.clear();
      _addressController.clear();
      _passwordController.clear();
      _facebookController.clear();
      _emailController.clear();
      _emergencyContactController.clear();
      setState(() {});
    }
  }

  Widget _buildIDSegment(TextEditingController controller, int length) {
    return SizedBox(
      width: length * 14.0,
      child: TextFormField(
        controller: controller,
        inputFormatters: [
          FilteringTextInputFormatter.digitsOnly,
          LengthLimitingTextInputFormatter(length),
        ],
        keyboardType: TextInputType.number,
        decoration: const InputDecoration(
          isDense: true,
          contentPadding: EdgeInsets.symmetric(vertical: 6),
          border: UnderlineInputBorder(),
        ),
        style: const TextStyle(fontSize: 16),
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
                      'Patient Form',
                      style: TextStyle(
                        fontSize: 26,
                        fontWeight: FontWeight.bold,
                        color: Colors.black87,
                      ),
                    ),
                    const SizedBox(height: 25),
                    Container(
                      padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
                      decoration: BoxDecoration(
                        color: Colors.white,
                        borderRadius: BorderRadius.circular(15),
                      ),
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          ShaderMask(
                            shaderCallback: (bounds) =>
                                AppColors.primaryGradient.createShader(bounds),
                            child: const Icon(Icons.badge, color: Colors.white, size: 20),
                          ),
                          const SizedBox(width: 8),
                          const Text('C', style: TextStyle(fontSize: 16)),
                          const SizedBox(width: 2),
                          _buildIDSegment(_idPart1Controller, 2),
                          const Text('-', style: TextStyle(fontSize: 16)),
                          _buildIDSegment(_idPart2Controller, 2),
                          const Text('-', style: TextStyle(fontSize: 16)),
                          _buildIDSegment(_idPart3Controller, 4),
                          const Text('-MAN', style: TextStyle(fontSize: 16)),
                          _buildIDSegment(_idPart4Controller, 3),
                        ],
                      ),
                    ),
                    const SizedBox(height: 20),
                    _buildInputField(Icons.person, 'First Name', controller: _firstNameController, requiredField: true),
                    _buildInputField(Icons.person_outline, 'Middle Name', controller: _middleNameController),
                    _buildInputField(Icons.people, 'Last Name', controller: _lastNameController, requiredField: true),
                    _buildInputField(Icons.home, 'Address', controller: _addressController, requiredField: true),
                    _buildInputField(Icons.lock, 'Password',
                        controller: _passwordController,
                        obscure: true,
                        requiredField: true,
                        hintText: 'Min 8 chars, 1 cap, 1 num, 1 special',
                        validator: (value) {
                          if (value == null || value.isEmpty) return 'Required';
                          final pattern = RegExp(r'^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$&*~]).{8,}$');
                          if (!pattern.hasMatch(value)) return 'Password must have 1 capital, 1 number, 1 special, min 8 chars';
                          return null;
                        }),
                    _buildInputField(Icons.facebook, 'Facebook', controller: _facebookController),
                    _buildInputField(Icons.email, 'Email',
                        controller: _emailController,
                        requiredField: true,
                        validator: (value) {
                          if (value == null || value.isEmpty) return 'Required';
                          if (!value.contains('@')) return 'Enter a valid email';
                          return null;
                        }),
                    _buildInputField(Icons.phone, 'Emergency Contact',
                        controller: _emergencyContactController,
                        requiredField: true,
                        prefixText: '63+ ',
                        hintText: 'Enter 10 digits',
                        keyboardType: TextInputType.number,
                        inputFormatters: [
                          FilteringTextInputFormatter.digitsOnly,
                          LengthLimitingTextInputFormatter(10),
                        ],
                        validator: (value) {
                          if (value == null || value.isEmpty) return 'Required';
                          if (value.length != 10) return 'Enter 10 digits';
                          return null;
                        }),
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
    String? prefixText,
    TextInputType? keyboardType,
    String? Function(String?)? validator,
  }) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: Container(
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(15),
        ),
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 16.0, vertical: 4.0),
          child: TextFormField(
            controller: controller,
            obscureText: obscure,
            inputFormatters: inputFormatters,
            keyboardType: keyboardType ?? TextInputType.text,
            validator: validator ?? (requiredField ? (value) => (value == null || value.trim().isEmpty) ? 'Required' : null : null),
            decoration: InputDecoration(
              prefixIcon: ShaderMask(
                shaderCallback: (bounds) =>
                    AppColors.primaryGradient.createShader(bounds),
                child: Icon(icon, color: Colors.white),
              ),
              prefixText: prefixText,
              hintText: hintText,
              labelText: label,
              labelStyle: const TextStyle(
                fontWeight: FontWeight.bold,
                fontSize: 16,
              ),
              border: InputBorder.none,
            ),
          ),
        ),
      ),
    );
  }
}
