import 'package:flutter/material.dart';
import '../widgets/app_colors.dart';
import '../mock/mock_nurse_form_db.dart';

class AddForm extends StatefulWidget {
  const AddForm({super.key});

  @override
  State<AddForm> createState() => _AddFormState();
}

class _AddFormState extends State<AddForm> {
  final _formKey = GlobalKey<FormState>();
  final _firstNameController = TextEditingController();
  final _middleNameController = TextEditingController();
  final _lastNameController = TextEditingController();
  final _addressController = TextEditingController();
  final _passwordController = TextEditingController();
  final _facebookController = TextEditingController();
  final _emailController = TextEditingController();
  final _emergencyContactController = TextEditingController();
  final _patientIdController = TextEditingController();
  int _lastPatientNumber = 0;

  @override
  void initState() {
    super.initState();
    _patientIdController.text = generatePatientID();
  }

  @override
  void dispose() {
    for (final c in [
      _firstNameController,
      _middleNameController,
      _lastNameController,
      _addressController,
      _passwordController,
      _facebookController,
      _emailController,
      _emergencyContactController,
      _patientIdController
    ]) {
      c.dispose();
    }
    super.dispose();
  }

  String generatePatientID() {
    _lastPatientNumber++;
    return 'R${_lastPatientNumber.toString().padLeft(3, '0')}';
  }

  void _submitForm() {
    if (_formKey.currentState?.validate() ?? false) {
      final form = NurseForm(
        id: _patientIdController.text,
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
      _patientIdController.text = generatePatientID();
      showDialog(
        context: context,
        builder: (context) => Dialog(
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
          child: Padding(
            padding: const EdgeInsets.all(24),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                Container(
                  width: 80,
                  height: 80,
                  decoration: const BoxDecoration(
                    gradient: AppColors.primaryGradient,
                    shape: BoxShape.circle,
                  ),
                  child: const Icon(Icons.done_all_rounded,
                      color: Colors.white, size: 50),
                ),
                const SizedBox(height: 20),
                Text(
                  "Form Submitted!",
                  style: const TextStyle(
                    fontSize: 22,
                    fontWeight: FontWeight.w700,
                    color: Color.fromARGB(221, 24, 129, 20),
                  ),
                  textAlign: TextAlign.center,
                ),
                const SizedBox(height: 12),
                Text(
                  "Patient ID: ${form.id}",
                  textAlign: TextAlign.center,
                  style: const TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                    color: Colors.black,
                  ),
                ),
              ],
            ),
          ),
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF5F7FA),
      appBar: AppBar(
        automaticallyImplyLeading: false,
        title: const Text(
          "Add Patient Form",
          style: TextStyle(fontWeight: FontWeight.w700, fontSize: 22),
        ),
        flexibleSpace: Container(
          decoration: const BoxDecoration(gradient: AppColors.primaryGradient),
        ),
        foregroundColor: Colors.white,
        elevation: 0,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 25),
        child: Form(
          key: _formKey,
          child: Column(
            children: [
              _buildSection(
                title: "Patient Record",
                icon: Icons.badge_rounded,
                children: [
                  _animatedTextField(
                    _patientIdController,
                    label: "Record ID",
                    readOnly: true,
                    requiredField: false,
                    boldText: true,
                  ),
                ],
              ),
              const SizedBox(height: 25),
              _buildSection(
                title: "Patient Information",
                icon: Icons.person_rounded,
                children: [
                  _animatedTextField(
                      _firstNameController,
                      label: "First Name",
                      icon: Icons.person_outline_rounded),
                  const SizedBox(height: 16),
                  _animatedTextField(
                      _middleNameController,
                      label: "Middle Name",
                      icon: Icons.person_outline_rounded,
                      requiredField: false),
                  const SizedBox(height: 16),
                  _animatedTextField(
                      _lastNameController,
                      label: "Last Name",
                      icon: Icons.person_outline_rounded),
                  const SizedBox(height: 16),
                  _animatedTextField(
                      _addressController,
                      label: "Address",
                      icon: Icons.home_rounded),
                ],
              ),
              const SizedBox(height: 25),
              _buildSection(
                title: "Contact Information",
                icon: Icons.phone_rounded,
                children: [
                  _animatedTextField(
                      _facebookController,
                      label: "Facebook",
                      icon: Icons.facebook_rounded,
                      requiredField: false),
                  const SizedBox(height: 16),
                  _animatedTextField(
                      _emailController,
                      label: "Email",
                      icon: Icons.email_rounded),
                  const SizedBox(height: 16),
                  _animatedTextField(
                      _emergencyContactController,
                      label: "Emergency Contact",
                      icon: Icons.call_rounded,
                      keyboard: TextInputType.number),
                ],
              ),
              const SizedBox(height: 35),
              _buildSubmitButton(),
            ],
          ),
        ),
      ),
    );
  }

  Widget _animatedTextField(
    TextEditingController controller, {
    required String label,
    IconData? icon,
    TextInputType keyboard = TextInputType.text,
    bool readOnly = false,
    bool requiredField = true,
    bool boldText = false, 
    int maxLines = 1,
  }) {
    final focusNode = FocusNode();
    return StatefulBuilder(
      builder: (context, setState) {
        focusNode.addListener(() => setState(() {}));
        return AnimatedContainer(
          duration: const Duration(milliseconds: 250),
          decoration: BoxDecoration(
            gradient: focusNode.hasFocus
                ? AppColors.primaryGradient
                : const LinearGradient(colors: [Colors.white, Colors.white]),
            borderRadius: BorderRadius.circular(15),
            boxShadow: focusNode.hasFocus
                ? [
                    BoxShadow(
                      color: AppColors.primaryGradient.colors.first.withOpacity(0.25),
                      blurRadius: 12,
                      offset: const Offset(0, 4),
                    )
                  ]
                : [],
          ),
          padding: const EdgeInsets.all(1.2),
          child: Container(
            decoration: BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.circular(14),
            ),
            child: TextFormField(
              focusNode: focusNode,
              controller: controller,
              keyboardType: keyboard,
              readOnly: readOnly,
              maxLines: maxLines,
              style: boldText
                  ? const TextStyle(
                      fontWeight: FontWeight.bold,
                      color: Colors.black,
                      fontSize: 16,
                    )
                  : null,
              validator: (value) =>
                  requiredField && (value == null || value.isEmpty)
                      ? "This field is required"
                      : null,
              decoration: InputDecoration(
                labelText: label,
                labelStyle: boldText
                    ? const TextStyle(
                        fontWeight: FontWeight.bold,
                        color: Colors.black,
                        fontSize: 16,
                      )
                    : TextStyle(
                        fontWeight: FontWeight.w500,
                        color: Colors.grey.shade600,
                      ),
                prefixIcon: icon != null
                    ? ShaderMask(
                        shaderCallback: (bounds) =>
                            AppColors.primaryGradient.createShader(bounds),
                        child: Icon(icon, color: Colors.white),
                      )
                    : null,
                border: InputBorder.none,
                contentPadding:
                    const EdgeInsets.symmetric(vertical: 18, horizontal: 16),
              ),
            ),
          ),
        );
      },
    );
  }

  Widget _buildSection(
      {required String title, required IconData icon, List<Widget>? children}) {
    return Container(
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(20),
        boxShadow: [
          BoxShadow(
            color: AppColors.primaryGradient.colors.first.withOpacity(0.08),
            blurRadius: 15,
            offset: const Offset(0, 5),
          ),
        ],
      ),
      padding: const EdgeInsets.all(24),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Container(
                padding: const EdgeInsets.all(10),
                decoration: BoxDecoration(
                  gradient: AppColors.primaryGradient,
                  borderRadius: BorderRadius.circular(12),
                ),
                child: Icon(icon, color: Colors.white, size: 24),
              ),
              const SizedBox(width: 15),
              Text(title,
                  style: const TextStyle(
                      fontSize: 18,
                      fontWeight: FontWeight.w700,
                      color: Colors.black87)),
            ],
          ),
          const Divider(height: 30, thickness: 1, color: Color(0xFFF5F7FA)),
          ...?children,
        ],
      ),
    );
  }

  Widget _buildSubmitButton() {
    return InkWell(
      onTap: _submitForm,
      borderRadius: BorderRadius.circular(18),
      child: Container(
        width: double.infinity,
        padding: const EdgeInsets.symmetric(vertical: 18),
        decoration: BoxDecoration(
          gradient: AppColors.primaryGradient,
          borderRadius: BorderRadius.circular(18),
          boxShadow: [
            BoxShadow(
              color: AppColors.primaryGradient.colors.first.withOpacity(0.5),
              offset: const Offset(0, 8),
              blurRadius: 20,
            ),
          ],
        ),
        child: const Center(
          child: Text(
            "Submit",
            style: TextStyle(
                color: Colors.white, fontWeight: FontWeight.w700, fontSize: 18),
          ),
        ),
      ),
    );
  }
}
