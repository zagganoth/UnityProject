SELECT user_id,MIN(last_login), lapsed_or_reactivated FROM lapsed_reactivated_users GROUP BY user_id

SELECT @c_val:=@c_val+SUM(spend_amount) as cumulative_spend, SUM(spend_amount), days_since_install

SELECT SUM(spend_amount)as total_spend,days_since_install
(SELECT ut.user_id,DATEDIFF(order_date,install_date) as days_after_install,spend_amount
FROM (SELECT user_id, order_date,spend_amount FROM master_purchase_table WHERE user_id IN
(SELECT user_id FROM user_custom_cohort WHERE cohort_id=’<desired_cohort_id>’)) ut
INNER JOIN user_data ud ON ud.user_id=ut.user_id) u
GROUP BY days_since_install
