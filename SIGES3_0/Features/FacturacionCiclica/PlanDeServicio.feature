Feature: Gestión de Planes de Servicio

@GeneracionPlanActivo
Scenario: Generación inicial del Plan (Inicio -> Activo)
	Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
	And Se ingresa al módulo 'Facturación Cíclica'
	And Se ingresa al submódulo 'Plan de Servicio'
	And Se selecciona 'Detalles del Plan'
	When Se configuran los límites de los comprobantes:
	| Campo          | Valor |
	| Valor mínimo   | 50    |
	| Valor máximo   | 500   |

	And Se configuran los límites de locales y usuarios:
	| Entidad  | Mínimo | Máximo |
	| Locales  | 1      | 5      |
	| Usuarios | 2      | 15     |

	And Se selecciona la pestaña 'Datos Generales'
	And Se ingresa la información básica del plan:
	| Campo           | Valor                                        |
	| Nombre del plan | Plan Agro                                    |
	| Descripción     | Plan orientadas a empresas agroindustriales. |

	And Se selecciona el ciclo de facturación 'MENSUAL'
	And Se ingresa el precio del plan '100'
	Then Se procede a 'GUARDAR' los cambios del plan
	And Se confirma el registro exitoso

@E2E_BajaPlan_Rechazada
Scenario: Crear plan y luego intentar darlo de baja (rechazado)
	Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
	And Se ingresa al módulo 'Facturación Cíclica'
	And Se ingresa al submódulo 'Plan de Servicio'
	When Se crea un nuevo plan con nombre dinámico

	# BÚSQUEDA
	When Se busca el plan creado
	And Se selecciona el plan en estado 'Activo'

	# INTENTO FALLIDO
	And Se hace clic en 'Solicitar Baja'
	And En el modal se selecciona 'No'

	Then Se valida que el estado del plan permanezca como 'Activo'

@E2E_BajaPlan_Exitosa
Scenario: Crear plan y darlo de baja correctamente
	Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
	And Se ingresa al módulo 'Facturación Cíclica'
	And Se ingresa al submódulo 'Plan de Servicio'

	# CREACIÓN
	When Se crea un nuevo plan con nombre dinámico

	# BÚSQUEDA
	When Se busca el plan creado
	And Se selecciona el plan en estado 'Activo'

	# BAJA EXITOSA
	And Se hace clic en 'Solicitar Baja'
	And En el modal se selecciona 'Si'

	Then Se confirma la operación exitosa
	Then Se valida que el estado del plan cambie a 'Dado de Baja'

@E2E_EditarPlan_ActualizacionExitosa
Scenario: Editar plan validando creación previa y confirmación con alerta OK
	Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
	And Se ingresa al módulo 'Facturación Cíclica'
	And Se ingresa al submódulo 'Plan de Servicio'

	# VALIDAR O CREAR
	When Se busca un plan existente
	And Si no existe un plan, se crea uno nuevo

	# EDICIÓN
	And Se selecciona el plan en estado 'Activo'
	And Se hace clic en 'Editar Plan'

	And Se selecciona el ciclo de facturación 'MENSUAL'
	And Se ingresa el nuevo monto '150'

	And Se configuran los límites de comprobantes:
	| Min | Max |
	| 50  | 200 |

	And Se configuran los límites de locales:
	| Min | Max |
	| 1   | 5   |

	# GUARDADO
	And Se hace clic en 'Guardar'

	# ALERTA
	And En la alerta de confirmación se hace clic en 'OK'

	Then Se valida que el plan fue actualizado correctamente

@E2E_EditarPlan_ActivarCicloAnual
Scenario: Editar plan y activar ciclo anual con validación automática de existencia
	Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
	And Se ingresa al módulo 'Facturación Cíclica'
	And Se ingresa al submódulo 'Plan de Servicio'

	# VALIDAR O CREAR
	When Se busca un plan existente
	And Si no existe un plan, se crea uno nuevo

	# EDICIÓN
	And Se selecciona el plan en estado 'Activo'
	And Se hace clic en 'Editar Plan'

	And Se selecciona el ciclo de facturación 'ANUAL'
	And Se ingresa el nuevo monto '1200'

	And Se configuran los límites de usuarios:
	| Min | Max |
	| 2   | 15  |

	And Se configuran los límites de locales:
	| Min | Max |
	| 1   | 5   |

	And Se hace clic en 'Guardar'

	# ALERTA
	And En la alerta de confirmación se hace clic en 'OK'

@DesactivarPlan
Scenario: Desactivación de un Plan (Activo -> Inactivo)
	Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
	And Se ingresa al módulo 'Facturación Cíclica'
	And Se ingresa al submódulo 'Plan de Servicio'
	When Se busca un plan existente
	And Si no existe un plan, se crea uno nuevo
	And Se selecciona el plan en estado 'Activo'
	And Se hace clic en el toggle de estado del plan
	And En el modal se selecciona 'Si'
	Then Se valida que el estado del plan cambie a 'Inactivo'

@CancelarDesactivacion
Scenario: Cancelar desactivación de un Plan (permanece Activo)
	Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
	And Se ingresa al módulo 'Facturación Cíclica'
	And Se ingresa al submódulo 'Plan de Servicio'
	When Se busca un plan existente
	And Si no existe un plan, se crea uno nuevo
	And Se selecciona el plan en estado 'Activo'
	And Se hace clic en el toggle de estado del plan
	And En el modal se selecciona 'No'
	Then Se valida que el estado del plan permanezca como 'Activo'

@ReactivarPlan
Scenario: Reactivación de un Plan (Inactivo -> Activo)
	Given el usuario ingresa al ambiente 'https://sigesdev.newfrontdev-qa.sigesonline.com/auth/login'
	When el usuario inicia sesión con usuario 'pamela.tone@recsa.com' y contraseña 'calidad'
	And Se ingresa al módulo 'Facturación Cíclica'
	And Se ingresa al submódulo 'Plan de Servicio'
	When Se busca un plan existente
	And Si no existe un plan, se crea uno nuevo
	And Se selecciona el plan en estado 'Activo'
	And Se hace clic en el toggle de estado del plan
	And En el modal se selecciona 'Si'
	And Se selecciona el plan en estado 'Inactivo'
	And Se hace clic en el toggle de estado del plan
	Then Se valida que el estado del plan cambie a 'Activo'