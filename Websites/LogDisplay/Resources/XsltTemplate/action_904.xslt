<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="age_custsubpwdupd2">
        <table class="tblDesc">
            <tr>
              <xsl:choose>
                <xsl:when test="@subaccount">
                  <td class="colW1">
                    {lbl:SubAccount}
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@subaccount" />
                  </td>
                </xsl:when>
                <xsl:when test="@subaccountid">
                  <td class="colW1">
                    {lbl:SubAccountID}
                  </td>
                  <td class="colW2">
                    <xsl:value-of select="@subaccountid" />
                  </td>
                </xsl:when>
              </xsl:choose>
              
                <td class="colW1">{lbl:IP}</td>
                <td class="colW2">
                    <xsl:value-of select="@ip" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">
                    
                </td>
            </tr>
        </table>
    </xsl:template>
  <xsl:template match="age_customerpwdupd2">
    <table class="tblDesc">
      <tr>
        <xsl:choose>
          <xsl:when test="@subaccount">
            <td class="colW1">
              {lbl:SubAccount}
            </td>
            <td class="colW2">
              <xsl:value-of select="@subaccount" />
            </td>
          </xsl:when>
          <xsl:when test="@subaccountid">
            <td class="colW1">
              {lbl:SubAccountID}
            </td>
            <td class="colW2">
              <xsl:value-of select="@subaccountid" />
            </td>
          </xsl:when>
        </xsl:choose>

        <td class="colW1">{lbl:IP}</td>
        <td class="colW2">
          <xsl:value-of select="@ip" />
        </td>
        <td class="colW1"></td>
        <td class="colW2">

        </td>
      </tr>
    </table>
  </xsl:template>
  
</xsl:stylesheet>
